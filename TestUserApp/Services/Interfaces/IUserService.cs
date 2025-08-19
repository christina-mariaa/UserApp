using TestUserApp.Models;

namespace TestUserApp.Services.Interfaces
{
    public interface IUserService
    {
        Task<IReadOnlyList<User>> GetAllAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<int> CreateUserAsync(string name, string? surname, int age, DateOnly? birthDate);
        Task<int> UpdateUserAsync(int id, string name, string? surname, int age, DateOnly? birthDate);
        Task<int> DeleteUserAsync(int id);
    }
}
