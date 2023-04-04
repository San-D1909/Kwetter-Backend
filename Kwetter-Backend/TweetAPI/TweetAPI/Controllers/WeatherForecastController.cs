using Microsoft.AspNetCore.Mvc;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace TweetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        ApplicationContext context;
        public WeatherForecastController(ApplicationContext context)
        {
            this.context = context;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            IConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = "host.docker.internal",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
            };
            using (IConnection connection = connectionFactory.CreateConnection())
            {
                IModel channel = connection.CreateModel();
                channel.ExchangeDeclare("userExchange", ExchangeType.Topic, true);

                channel.QueueDeclare("user", true, false, false, null);
                channel.QueueBind("user", "userExchange", "userDemo");

                var consumer = new EventingBasicConsumer(channel);
                // define a callback function for incoming messages
                consumer.Received += (s, e) =>
                {
                    context.Tweet.Add(new Models.Tweet { Body = e.ToString(), CreatedAt = DateTime.Now, Header = "testheade3", IsArchived = false, IsDisabled = false, UserId = Guid.NewGuid() });
                    context.SaveChanges();
                };

                channel.BasicConsume("user", true, consumer);
            }
            return Ok();
        }
        void Consumer_Received(object? sender, BasicDeliverEventArgs e)
        {
            context.Tweet.Add(new Models.Tweet { Body = "dit moet werken", CreatedAt = DateTime.Now, Header = "testheade3", IsArchived = false, IsDisabled = false, UserId = Guid.NewGuid() });
            context.SaveChanges();
        }
    }
}