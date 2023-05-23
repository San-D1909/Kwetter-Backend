using UserProfileAPI.Models;

namespace UserProfileAPI.Services.Interfaces
{
    public interface IUsersRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User?> GetUserById(string userId);
        Task<User> CreateUser(User user);
        Task<User> UpdateUser(User user);
        Task<int> DeleteUser(string userId);
    }
}
