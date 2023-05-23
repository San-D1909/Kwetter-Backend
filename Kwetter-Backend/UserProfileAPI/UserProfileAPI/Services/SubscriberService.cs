using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using UserProfileAPI.Services.Interfaces;

namespace UserProfileAPI.Services
{
    public class SubscriberService : ISubscriberService
    {
        public void GetFromQueue()
        {
           string exchange = "TweetExchange";
            string routingKey = "tweetDemo";
            string queue = "tweet";

            IConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = "iqadot.switzerlandnorth.cloudapp.azure.com",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
            };
            IConnection connection = connectionFactory.CreateConnection();

            IModel channel = connection.CreateModel();
            channel.ExchangeDeclare(exchange, ExchangeType.Topic, true);

            channel.QueueDeclare(queue, true, false, false, null);
            channel.QueueBind(queue, exchange, routingKey);

            var consumer = new EventingBasicConsumer(channel);
            // define a callback function for incoming messages
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.Unicode.GetString(body);
                Console.WriteLine("Received message: {0}", message);
            };
            channel.BasicConsume(queue, true, consumer);
            return;
        }
    }
}
