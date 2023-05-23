using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using TweetAPI;
using TweetAPI.Models;
using TweetAPI.Services;
using TweetAPI.Services.Interfaces;

namespace KwetterTestTweets
{
    [TestFixture]
    public class UnitTestTweets
    {
        private DbContextOptions<ApplicationContext> _options;
        private ITweetRepository _tweetRepository;

        [SetUp]
        public void Setup()
        {
            // Set up the in-memory database options
            _options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            // Initialize the repository
            _tweetRepository = new TweetRepository(new ApplicationContext(_options));
        }

        [TearDown]
        public void Cleanup()
        {
            // Clean up the in-memory database after each test
            using (var context = new ApplicationContext(_options))
            {
                context.Database.EnsureDeleted();
            }
        }

        [Test]
        public async Task CreateTweet_Should_CreateNewTweet()
        {
            // Arrange
            var tweet = new Tweet
            {
                UserId = "user123",
                Body = "This is a test tweet."
            };

            // Act
            var createdTweet = await _tweetRepository.CreateTweet(tweet);

            // Assert
            Assert.IsNotNull(createdTweet);
            Assert.AreEqual(tweet.UserId, createdTweet.UserId);
            Assert.AreEqual(tweet.Body, createdTweet.Body);
            Assert.IsFalse(createdTweet.IsArchived);
            Assert.IsFalse(createdTweet.IsDisabled);
            Assert.AreNotEqual(default(DateTime), createdTweet.CreatedAt);
        }

        [Test]
        public async Task DeleteTweet_Should_RemoveTweet()
        {
            // Arrange
            var tweetId = Guid.NewGuid();
            var tweet = new Tweet
            {
                TweetId = tweetId,
                UserId = "user123",
                Body = "This is a test tweet."
            };

            using (var context = new ApplicationContext(_options))
            {
                context.Tweet.Add(tweet);
                await context.SaveChangesAsync();
            }

            // Act
            await _tweetRepository.DeleteTweet(tweetId);

            // Assert
            using (var context = new ApplicationContext(_options))
            {
                var deletedTweet = await context.Tweet.FindAsync(tweetId);
                Assert.IsNull(deletedTweet);
            }
        }
    }
}
