
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using smart_meter.Model.DTOs;
using System.Text;

namespace smart_meter.Services
{
    public class RabbitMqServices 
    {
        public readonly ConnectionFactory _connectionFactory;

        public RabbitMqServices(IConfiguration config)
        {
            var connection = new ConnectionFactory
            {
                HostName = config["RabbitMq:HostName"]
            };
            _connectionFactory = connection;
        }

        public async Task<ConsumerCreateDto> sendMesageDirectExchange(string exchangName, string queueName, string routingKey,ConsumerCreateDto message)
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: exchangName, type: ExchangeType.Direct, autoDelete: false);
            await channel.QueueDeclareAsync(queue: queueName, true, false, false);
            await channel.QueueBindAsync(queue: queueName, exchange: exchangName, routingKey: routingKey);

            var json = System.Text.Json.JsonSerializer.Serialize(message);
            var body =Encoding.UTF8.GetBytes( json);

            await channel.BasicPublishAsync(
                exchange: exchangName,
                routingKey: routingKey,
                body: body
                );
            
            return message;
        }

        

    }
}
