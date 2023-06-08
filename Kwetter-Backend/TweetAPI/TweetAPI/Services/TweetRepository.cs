using Microsoft.EntityFrameworkCore;
using TweetAPI.Models;
using TweetAPI.Services.Interfaces;

namespace TweetAPI.Services
{
    public class TweetRepository : ITweetRepository
    {
        private readonly ApplicationContext context;

        public TweetRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Tweet>> GetAllTweets()
        {
            return await this.context.Tweet.OrderByDescending(x => x.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<Tweet>> GetTweetById(string userId)
        {
            return await this.context.Tweet.Where(x => x.UserId != userId).OrderByDescending(x => x.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<Tweet>> GetTweetsByUser(string userId)
        {
            return await this.context.Tweet.Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAt).ToListAsync();
        }

        public async Task<Tweet> CreateTweet(Tweet tweet)
        {
            tweet.IsArchived = false;
            tweet.IsDisabled = false;
            tweet.CreatedAt = DateTime.Now;

            await this.context.Tweet.AddAsync(tweet);
            await this.context.SaveChangesAsync();

            return tweet;
        }

        public async Task<Tweet> UpdateTweet(Tweet tweet)
        {
            this.context.Tweet.Update(tweet);
            await this.context.SaveChangesAsync();
            return tweet;
        }

        public async Task<int> DeleteTweet(Guid tweetId)
        {
            var tweet = await this.context.Tweet.FirstOrDefaultAsync(x => x.TweetId == tweetId);
            if (tweet != null)
            {
                this.context.Tweet.Remove(tweet);
                return await this.context.SaveChangesAsync();
            }
            return 0;
        }
    }

}
