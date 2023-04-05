using Microsoft.AspNetCore.Mvc;
using TweetAPI.Services;
using TweetAPI.Services.Interfaces;

namespace TweetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            IPublisherService publisherService = new PublisherService();
            publisherService.Publish();
            return null;
        }
    }
}