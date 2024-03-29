﻿using TweetAPI.Models;

namespace TweetAPI.Services.Interfaces
{
    public interface ITweetRepository
    {
        Task<IEnumerable<Tweet>> GetAllTweets();
        Task<IEnumerable<Tweet>> GetTweetById(string userId);
        Task<IEnumerable<Tweet>> GetTweetsByUser(string userId);
        Task<Tweet> CreateTweet(Tweet tweet);
        Task<Tweet> UpdateTweet(Tweet tweet);
        Task<int> DeleteTweet(Guid tweetId);
    }

}
