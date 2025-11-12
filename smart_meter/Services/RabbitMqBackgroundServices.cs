
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using smart_meter.Model.DTOs;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace smart_meter.Services
{
    public class RabbitMqBackgroundServices : BackgroundService
    {
        public readonly RabbitMqServices _rabbitMqServices;
        public readonly IServiceScopeFactory _scopeFactory;
        //public readonly MeterService _meterService;


        public readonly ConcurrentQueue<MeterReading> _bufferReadings = new();

        private readonly TimeSpan _flushInterval = TimeSpan.FromSeconds(15);



        public RabbitMqBackgroundServices(RabbitMqServices rabbitMqServices, IServiceScopeFactory scopeFactory)
        {
            _rabbitMqServices = rabbitMqServices;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                using var connection = await _rabbitMqServices._connectionFactory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                //await channel.ExchangeDeclareAsync(exchange: "Connection", type: ExchangeType.Direct, autoDelete: false);
                //await channel.QueueDeclareAsync(queue: "Requestconnection", true, false, false);
                //await channel.QueueBindAsync(queue: "Requestconnection", exchange: "Connection", routingKey: "Requestconnection");

                await channel.QueueBindAsync(queue: "meter_reading", exchange: "smart_meter", routingKey: "meter_reading");

                var consumer = new AsyncEventingBasicConsumer(channel);

                consumer.ReceivedAsync +=  async (model, er) =>
                {
                    var json = Encoding.UTF8.GetString(er.Body.ToArray());

                    var data = JsonSerializer.Deserialize<MeterReading>(json);


                    if (data != null)
                    {
                        var reading = new MeterReading
                        {
                            Meterserialno = data.Meterserialno,
                            Energyconsumed = data.Energyconsumed,
                            Readingdatetime = data.Readingdatetime,
                            Voltage = data.Voltage,
                            Current = data.Current
                        };

                        _bufferReadings.Enqueue(data);
                    }

                    await Task.Yield();

                };

                await channel.BasicConsumeAsync(queue: "meter_reading", autoAck: true, consumer: consumer);

                Console.WriteLine("Rabbitmq listining the connection request....");

                while (!stoppingToken.IsCancellationRequested)
                {
                    Console.WriteLine("enter");
                    await Task.Delay(_flushInterval, stoppingToken);
                    await Flushdata();
                    Console.WriteLine("exit");

                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }
        private async Task Flushdata()
        {
            if(_bufferReadings.IsEmpty) return;

            var list = new List<MeterReading>();

            while (_bufferReadings.TryDequeue(out var item))
            {
                list.Add(item);
            }

            if(list.Count > 0) { return; }


            using var scope = _scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<MeterService>();

            await service.InsertMeterData(list);
            Console.WriteLine("Data Inserted");
        }
    }
}
