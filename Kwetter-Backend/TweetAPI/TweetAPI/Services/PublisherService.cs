using RabbitMQ.Client;
using System.Text;
using TweetAPI.Services.Interfaces;

namespace TweetAPI.Services
{
    public class PublisherService : IPublisherService
    {
        public void Publish() {
            string exchange = "TweetExchange";
            string routingKey = "tweetDemo";
            string message = "Hello, RabbitMQ! From TweetService";

            IConnectionFactory factory = new ConnectionFactory { HostName = "host.docker.internal", Port = 5672, Password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD"), UserName = "guest" };
            var connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();
            // declare a queue
            channel.ExchangeDeclare(exchange, ExchangeType.Topic, true);

            // create a message
            byte[] body = Encoding.Unicode.GetBytes(message);

            // publish the message to the queue
            channel.BasicPublish(exchange, routingKey, null, body);      
        }
    }
}
