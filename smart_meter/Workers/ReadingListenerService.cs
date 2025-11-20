using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using smart_meter.Model.DTOs;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace smart_meter.Worker
{
    public class ReadingListenerService : BackgroundService
    {
        private readonly ILogger<ReadingListenerService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private IConnection? _connection;
        private IChannel? _channel;
        private const string QueueName = "meter_readings_queue";

        public ReadingListenerService(ILogger<ReadingListenerService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost",
                    UserName = "smart-proj",
                    Password = "1234",
                    VirtualHost = "smart-dev",
                };

                _connection = await factory.CreateConnectionAsync(stoppingToken);
                _channel = await _connection.CreateChannelAsync(options: null, cancellationToken: stoppingToken);

                await _channel.QueueDeclareAsync(queue: QueueName,durable: true,exclusive: false,autoDelete: false, cancellationToken: stoppingToken);
                await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(_channel);

                consumer.ReceivedAsync += async (sender, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);

                    try
                    {
                        var data = JsonSerializer.Deserialize<MeterReading>(json);
                        if (data is null)
                        {
                            await _channel.BasicNackAsync(ea.DeliveryTag, false, false);
                            return;
                        }

                        _logger.LogInformation("Received Meter {MeterId}, Energy {Energy}",
                            data.Meterserialno, data.Energyconsumed);

                        using var scope = _serviceProvider.CreateScope();
                        var dbService = scope.ServiceProvider.GetRequiredService<DatabaseService>();
                        await dbService.InsertMeterReadingAsync(data);

                        await _channel.BasicAckAsync(ea.DeliveryTag, false);
                        _logger.LogInformation("Saved and acknowledged.");
                    }
                    catch (Exception ex)
                    {   
                        _logger.LogError(ex, "Failed to process message.");
                        await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
                    }
                };

                await _channel.BasicConsumeAsync(queue: QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RabbitMQ consumer failed.");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping consumer...");
            if (_channel != null)
                await _channel.CloseAsync(200, "Service stopping", false, cancellationToken);
            if (_connection != null)
                await _connection.CloseAsync(200, "Service stopping", TimeSpan.FromSeconds(5), false, cancellationToken);
            await base.StopAsync(cancellationToken);
        }
    }
}
