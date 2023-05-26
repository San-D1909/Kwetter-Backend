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

            IConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri("#{RMQCONNECTIONSTRING}#");

            var connection = connectionFactory.CreateConnection();
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
