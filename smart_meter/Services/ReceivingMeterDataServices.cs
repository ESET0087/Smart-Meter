using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace smart_meter.Services
{
    public class ReceivingMeterDataServices
    {
        public async Task ReceiveData()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/"
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueBindAsync(
                queue: "meter_reading",
                exchange: "smart_meter",
                routingKey: "meter_reading"
                );

            var consumer =  new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (Model, er) =>
            {
                var body = er.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };

            await channel.BasicConsumeAsync(queue:"meter_reading", autoAck: true, consumer: consumer);

            
        }
    }
}
