namespace TweetAPI
{
    using TweetAPI.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Channels;

    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Tweet> Tweet { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<LikedTweet> LikedTweet { get; set; }
    }
}
