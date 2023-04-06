using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using UserProfileAPI.Services;
using UserProfileAPI.Services.Interfaces;

namespace UserProfileAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            IPublisherService publisherService = new PublisherService();
            publisherService.Publish();
            return Ok();
        }
    }
}