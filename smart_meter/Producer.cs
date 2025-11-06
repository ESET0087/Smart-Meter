using RabbitMQ.Client;

namespace smart_meter
{
    public class Producer
    {
        private readonly string _hostname;
        private readonly string _username;
        private readonly string _password;
        private readonly string _virtualHost;
        public Producer(string hostname, string username, string password, string virtualHost)
        {
            _hostname = hostname;
            _username = username;
            _password = password;
            _virtualHost = virtualHost;
        }
        public async void sendMessage(string message)
        {
                       var factory = new ConnectionFactory()
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password,
                VirtualHost = _virtualHost
            };
            using var connection = await factory.CreateConnectionAsync();
            using (var channel = connection.cre
            {
                channel.QueueDeclare(queue: "meter_readings",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                var body = System.Text.Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "",
                                     routingKey: "meter_readings",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }
        }
    }
}
