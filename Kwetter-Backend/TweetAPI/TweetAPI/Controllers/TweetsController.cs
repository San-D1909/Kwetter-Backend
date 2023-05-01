using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TweetAPI.Models;

namespace TweetAPI.Controllers
{
    [ApiController]
    [Route("api/tweetapi/[controller]")]
    public class TweetsController : Controller
    {
        private readonly ApplicationContext context;

        public TweetsController(ApplicationContext context)
        {
            this.context = context;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(string? userId)
        {
            try
            {
                if (this.context == null)
                {
                    return this.BadRequest("Error: Database context is null.");
                }

                var tweets = await this.context.Tweet.Where(x => x.UserId != userId).OrderByDescending(x => x.CreatedAt).ToListAsync();

                if (tweets == null || !tweets.Any())
                {
                    return this.NotFound("No tweets found.");
                }

                return this.Ok(tweets);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{TweetId:guid}")]
        public async Task<IActionResult> GetById(Guid TweetId)
        {
            return this.Ok(await this.context.Tweet.Where(x => x.TweetId == TweetId).FirstOrDefaultAsync());
        }
        
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(string? userId)
        {
            return this.Ok(await this.context.Tweet.Where(x => x.UserId == userId).OrderByDescending(x=>x.CreatedAt).ToListAsync());
        }

        [HttpPost("")]
        public async Task<IActionResult> Create(Tweet tweet)
        {
            tweet.IsArchived = false;
            tweet.IsDisabled = false;
            tweet.CreatedAt = DateTime.Now;
            await this.context.Tweet.AddAsync(tweet);
            await this.context.SaveChangesAsync();
            return this.Ok(tweet);
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
