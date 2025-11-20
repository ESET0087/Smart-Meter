using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using RabbitMQ.Client;
using Producer.Models;

class Program
{
    private const string QueueName = "meter_readings_queue";

    static async Task<int> Main(string[] args)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "smart-proj",
            Password = "1234",
            VirtualHost = "smart-dev"
        };

        try
        {
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            // Declare durable queue so it exists if consumer starts first
            await channel.QueueDeclareAsync(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var totalEnergy = 0m;
            var startTime = DateTime.UtcNow.Date.AddHours(0);

            for (int i = 0; i < 10; i++)
            {
                var readingTime = startTime.AddMinutes(15 * i);

                // energy keeps increasing slightly
                var energyIncrement = Math.Round(DecimalRandom(0.1, 1), 3);
                totalEnergy += energyIncrement;

                var reading = new MeterReading
                {
                    Meterserialno = "string",
                    Readingdatetime = readingTime,
                    Energyconsumed = totalEnergy,
                    Voltage = Math.Round(DecimalRandom(200, 240), 2),
                    Current = Math.Round(DecimalRandom(0, 10), 3)
                };

                var json = JsonSerializer.Serialize(reading);
                var body = Encoding.UTF8.GetBytes(json);

                var props = new RabbitMQ.Client.BasicProperties
                {
                    DeliveryMode = (DeliveryModes)2 // persistent
                };

                await channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: QueueName,
                    mandatory: false,
                    basicProperties: props,
                    body: body
                );

                Console.WriteLine($"Sent: {reading.Meterserialno} {reading.Energyconsumed} kWh at {reading.Readingdatetime:O}");
                Thread.Sleep(50); 
            }

            Console.WriteLine("Done publishing. Press any key to exit.");
            Console.ReadKey();
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error publishing messages: {ex.Message}");
            return 1;
        }
    }

    private static decimal DecimalRandom(double min, double max)
    {
        var rnd = new Random();
        return (decimal)(min + rnd.NextDouble() * (max - min));
    }
}