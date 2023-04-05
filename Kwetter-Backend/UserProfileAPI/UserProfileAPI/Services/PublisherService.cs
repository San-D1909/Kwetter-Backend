using RabbitMQ.Client;
using System.Text;
using UserProfileAPI.Services.Interfaces;

namespace UserProfileAPI.Services
{
    public class PublisherService : IPublisherService
    {
        public void Publish()
        {
            string exchange = "userExchange";
            string routingKey = "userDemo";
            string message = "Hello, RabbitMQ! From userService";

            IConnectionFactory factory = new ConnectionFactory { HostName = "host.docker.internal", Port = 5672, Password = "guest", UserName = "guest" };
            using (var connection = factory.CreateConnection())
            {
                IModel channel = connection.CreateModel();
                // declare a queue
                channel.ExchangeDeclare(exchange, ExchangeType.Topic, true);

                // create a message
                byte[] body = Encoding.Unicode.GetBytes(message);

                // publish the message to the queue
                channel.BasicPublish(exchange, routingKey, null, body);
            };
        }
    }
}
