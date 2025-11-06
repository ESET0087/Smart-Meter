
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using smart_meter.Model.DTOs;
using System.Text;
using System.Text.Json;

namespace smart_meter.Services
{
    public class RabbitMqBackgroundServices : BackgroundService
    {
        public readonly RabbitMqServices _rabbitMqServices;

        public RabbitMqBackgroundServices(RabbitMqServices rabbitMqServices)
        {
            _rabbitMqServices = rabbitMqServices;
            //Console.WriteLine("started under class");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("started");
            using var connection = await _rabbitMqServices._connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: "Connection", type: ExchangeType.Direct, autoDelete: false);
            await channel.QueueDeclareAsync(queue: "Requestconnection", true, false, false);
            await channel.QueueBindAsync(queue: "Requestconnection", exchange: "Connection", routingKey: "Requestconnection");

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync +=async (model, er) =>
            {
                var messages = Encoding.UTF8.GetString(er.Body.ToArray());

                var json = JsonSerializer.Deserialize<ConsumerCreateDto>(messages);
                
                Type type = json.GetType();
                Console.WriteLine(messages);
                Console.WriteLine(type);
                await Task.Yield();
                
            };
            
            await channel.BasicConsumeAsync(queue:"Requestconnection", autoAck: true, consumer: consumer);

            Console.WriteLine("Rabbitmq listining the connection request.....");
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
