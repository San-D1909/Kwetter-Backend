using RabbitMQ.Client;
using System.Text;
using UserProfileAPI.Services.Interfaces;

namespace UserProfileAPI.Services
{
    public class PublisherService : IPublisherService
    {
        public void DeleteUser(string userID)
        {
           string exchange = "userExchange";
            string routingKey = "userDelete";

            IConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.Uri = new Uri("#{RMQCONNECTIONSTRING}#");
            
            using (var connection = connectionFactory.CreateConnection())
            {
                IModel channel = connection.CreateModel();
                // declare a queue
                channel.ExchangeDeclare(exchange, ExchangeType.Topic, true);

                // create a message
                byte[] body = Encoding.Unicode.GetBytes(userID.ToString());

                // publish the message to the queue
                channel.BasicPublish(exchange, routingKey, null, body);
            };
        }
    }
}
