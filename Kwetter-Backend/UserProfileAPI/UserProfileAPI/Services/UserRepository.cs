using Microsoft.EntityFrameworkCore;
using UserProfileAPI.Models;
using UserProfileAPI.Services.Interfaces;

namespace UserProfileAPI.Services
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationContext context;

        public UsersRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await this.context.User.ToListAsync();
        }

        public async Task<User> GetUserById(string userId)
        {
            return await this.context.User.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<User> CreateUser(User user)
        {
            user.AddedAt = DateTime.Now;
            await this.context.User.AddAsync(user);
            await this.context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUser(User user)
        {
            this.context.User.Update(user);
            await this.context.SaveChangesAsync();
            return user;
        }

        public async Task<int> DeleteUser(string userId)
        {
            var user = await this.context.User.FirstOrDefaultAsync(x => x.UserId == userId);
            if (user != null)
            {
                this.context.User.Remove(user);
                return await this.context.SaveChangesAsync();
            }
            return 0;
        }
    }
}
