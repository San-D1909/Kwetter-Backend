using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TweetAPI.Models;
using TweetAPI.Services.Interfaces;

namespace TweetAPI.Controllers
{
    [ApiController]
    [Route("api/tweetapi/[controller]")]
    public class TweetsController : Controller
    {
        private readonly ITweetRepository tweetRepository;

        public TweetsController(ITweetRepository tweetRepository)
        {
            this.tweetRepository = tweetRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tweets = await this.tweetRepository.GetAllTweets();

            if (tweets == null || !tweets.Any())
            {
                return this.NotFound("No tweets found.");
            }

            return this.Ok(tweets);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById(string userId)
        {
            var tweets = await this.tweetRepository.GetTweetById(userId);

            if (tweet == null)
            {
                return this.NotFound("No timeline found!");
            }

            return this.Ok(tweet);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            var tweets = await this.tweetRepository.GetTweetsByUser(userId);

            if (tweets == null || !tweets.Any())
            {
                return this.NotFound("No tweets found for the specified user.");
            }

            return this.Ok(tweets);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create(Tweet tweet)
        {
            var createdTweet = await this.tweetRepository.CreateTweet(tweet);
            return this.Ok(createdTweet);
        }

        [HttpPut("")]
        public async Task<IActionResult> Update(Tweet tweet)
        {
            var updatedTweet = await this.tweetRepository.UpdateTweet(tweet);
            return this.Ok(updatedTweet);
        }

        [HttpDelete("{tweetId:guid}")]
        public async Task<IActionResult> Delete(Guid tweetId)
        {
            var affectedRows = await this.tweetRepository.DeleteTweet(tweetId);

            if (affectedRows > 0)
            {
                return this.Ok($"Deleted {affectedRows} tweet(s).");
            }

            return this.NotFound("Tweet not found.");
        }
    }
}
