using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TweetAPI.Models;

namespace TweetAPI.Controllers
{
    [ApiController]
    [Route("api/Tweets/[controller]")]
    public class TweetsController : Controller
    {
        private readonly ApplicationContext context;

        public TweetsController(ApplicationContext context)
        {
            this.context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            return this.Ok(await this.context.Tweet.ToListAsync());
        }

        [HttpGet("{TweetId:guid}")]
        public async Task<IActionResult> GetById(Guid TweetId)
        {
            return this.Ok(await this.context.Tweet.Where(x => x.TweetId == TweetId).FirstOrDefaultAsync());
        }

        [HttpPost("")]
        public async Task<IActionResult> Create(Tweet Tweet)
        {
            await this.context.Tweet.AddAsync(Tweet);
            await this.context.SaveChangesAsync();
            return this.Ok(Tweet);
        }

        [HttpPut("")]
        public async Task<IActionResult> Update(Tweet Tweet)
        {
            this.context.Tweet.Update(Tweet);
            await this.context.SaveChangesAsync();
            return this.Ok(Tweet);
        }

        [HttpDelete("{TweetId:guid}")]
        public async Task<IActionResult> Delete(Guid TweetId)
        {
            var tweet = await this.context.Tweet.Where(x => x.TweetId == TweetId).FirstAsync();
            this.context.Tweet.Remove(tweet);
            return this.Ok(await this.context.SaveChangesAsync());
        }
    }
}
