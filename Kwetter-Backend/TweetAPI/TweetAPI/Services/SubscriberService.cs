using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using TweetAPI.Services.Interfaces;
using TweetAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace TweetAPI.Services
{
    public class SubscriberService : ISubscriberService
    {
        private readonly ApplicationContext context;

        public SubscriberService(ApplicationContext context)
        {
            this.context = context;
        }
        public void GetDeletedFromQueue()
        {
            string exchange = "userExchange";
            string routingKey = "userDelete";
            string queue = "user";

            IConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = "host.docker.internal",
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
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                Guid message = Guid.Parse(Encoding.Unicode.GetString(body));
                var tweet = await this.context.Tweet.Where(x => x.TweetId == message).FirstAsync();
                this.context.Tweet.Remove(tweet);
                await this.context.SaveChangesAsync();
            };
            channel.BasicConsume(queue, true, consumer);
            return;
        }
    }
}
