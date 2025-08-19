using TestUserApp.Models;

namespace TestUserApp.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IReadOnlyList<User>> GetAllAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<int> CreateUserAsync(User user);
        Task<int> UpdateUserAsync(int id, User user);
        Task<int> DeleteUserAsync(int id);
    }
}
