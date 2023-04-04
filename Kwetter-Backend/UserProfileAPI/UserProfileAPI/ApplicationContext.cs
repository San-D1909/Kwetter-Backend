namespace UserProfileAPI
{
    using UserProfileAPI.Models;
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

        public DbSet<User> User { get; set; }
        public DbSet<Friend> Friend { get; set; }

    }
}
