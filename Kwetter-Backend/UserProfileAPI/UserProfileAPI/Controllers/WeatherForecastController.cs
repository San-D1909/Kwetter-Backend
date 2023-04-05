using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

namespace UserProfileAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            IConnectionFactory factory = new ConnectionFactory { HostName = "host.docker.internal", Port = 5672, Password = "guest", UserName = "guest" };
            using (var connection = factory.CreateConnection())
            {
                IModel channel = connection.CreateModel();
                // declare a queue
                channel.ExchangeDeclare("userExchange", ExchangeType.Topic, true);

                // create a message
                string message = "Hello, RabbitMQ from endpoint!";
                byte[] body = Encoding.Unicode.GetBytes(message);

                // publish the message to the queue
                channel.BasicPublish("userExchange", "userDemo", null, body);
            }
            return null;
        }
    }
}