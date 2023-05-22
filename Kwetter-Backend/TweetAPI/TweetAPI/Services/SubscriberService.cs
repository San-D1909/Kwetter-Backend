using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using TweetAPI.Services.Interfaces;
using TweetAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TweetAPI.Services
{
    public class SubscriberService : ISubscriberService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public SubscriberService(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public void GetDeletedFromQueue()
        {
      /*      string exchange = "userExchange";
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
                string message = Encoding.Unicode.GetString(body);

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                    var tweets = await dbContext.Tweet.Where(x => x.UserId == message).ToListAsync();

                    if (tweets != null && tweets.Count() != 0)
                    {
                        foreach (var x in tweets)
                        {
                            dbContext.Tweet.Remove(x);
                        }
                        await dbContext.SaveChangesAsync();
                    }
                }
            };

            channel.BasicConsume(queue, true, consumer);
            return;
        */
            }
    }
}
