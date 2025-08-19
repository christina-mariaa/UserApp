using TestUserApp.Models;
using TestUserApp.Repositories.Interfaces;
using TestUserApp.Services.Interfaces;

namespace TestUserApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<IReadOnlyList<User>> GetAllAsync()
        {
            return _userRepository.GetAllAsync();
        }

        public Task<User?> GetUserByIdAsync(int id)
        {
            return id <= 0 ? Task.FromResult<User?>(null) : _userRepository.GetUserByIdAsync(id);
        }

        public async Task<int> CreateUserAsync(string name, string? surname, int age, DateOnly? birthDate)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("Name required");
            return await _userRepository.CreateUserAsync(
                new User
                {
                    Name = name,
                    Surname = surname,
                    Age = age,
                    BirthDate = birthDate
                }
            );
        }

        public Task<int> UpdateUserAsync(int id, string name, string? surname, int age, DateOnly? birthDate)
        {
            return id <= 0 ? Task.FromResult(0) : _userRepository.UpdateUserAsync(
                id,
                new User
                {
                    Name = name,
                    Surname = surname,
                    Age = age,
                    BirthDate = birthDate
                }
            );
        }

        public Task<int> DeleteUserAsync(int id)
        {
            return id <= 0 ? Task.FromResult(0) : _userRepository.DeleteUserAsync(id);
        }
    }
}
