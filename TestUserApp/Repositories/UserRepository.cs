using Microsoft.Data.SqlClient;
using TestUserApp.Models;
using TestUserApp.Repositories.Interfaces;
using System.Data;

namespace TestUserApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public UserRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IReadOnlyList<User>> GetAllAsync()
        {
            using var connection = await _connectionFactory.CreateOpenAsync();
            using var command = new SqlCommand("Get_All", connection) { CommandType = CommandType.StoredProcedure};
            using var reader = await command.ExecuteReaderAsync();

            var user_list = new List<User>();
            while (await reader.ReadAsync())
            {
                user_list.Add(new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Surname = reader.IsDBNull(reader.GetOrdinal("surname")) ? null : reader.GetString(reader.GetOrdinal("surname")),
                    Age = reader.GetInt32(reader.GetOrdinal("age")),
                    BirthDate = reader.IsDBNull(reader.GetOrdinal("birth_date")) ? null : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("birth_date")))
                });
            }
            return user_list;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            using var connection = await _connectionFactory.CreateOpenAsync();
            using var command = new SqlCommand("User_Get_By_Id", connection) { CommandType = CommandType.StoredProcedure};
            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;

            return new User
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                Surname = reader.IsDBNull(reader.GetOrdinal("surname")) ? null : reader.GetString(reader.GetOrdinal("surname")),
                Age = reader.GetInt32(reader.GetOrdinal("age")),
                BirthDate = reader.IsDBNull(reader.GetOrdinal("birth_date")) ? null : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("birth_date")))
            };
        }

        public async Task<int> CreateUserAsync(User user)
        {
            using var connection = await _connectionFactory.CreateOpenAsync();
            using var command = new SqlCommand("Insert_User", connection) { CommandType = CommandType.StoredProcedure};
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Surname", (object?)user.Surname ?? DBNull.Value);
            command.Parameters.AddWithValue("@Age", user.Age);
            command.Parameters.AddWithValue("@Birth_date", (object?)user.BirthDate ?? DBNull.Value);

            var outId = new SqlParameter("@New_id", SqlDbType.Int) { Direction = ParameterDirection.Output };
            command.Parameters.Add(outId);

            await command.ExecuteNonQueryAsync();
            return (int)outId.Value;
        }

        public async Task<int> UpdateUserAsync(int id, User user)
        {
            using var connection = await _connectionFactory.CreateOpenAsync();
            using var command = new SqlCommand("Update_User", connection) { CommandType = CommandType.StoredProcedure};

            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Surname", (object?)user.Surname ?? DBNull.Value);
            command.Parameters.AddWithValue("@Age", user.Age);
            command.Parameters.AddWithValue("@Birth_date", (object?)user.BirthDate ?? DBNull.Value);

            var affected = (int)(await command.ExecuteScalarAsync() ?? 0);
            return affected;
        }

        public async Task<int> DeleteUserAsync(int id)
        {
            using var connection = await _connectionFactory.CreateOpenAsync();
            using var command = new SqlCommand("Delete_User", connection) { CommandType = CommandType.StoredProcedure};
            command.Parameters.AddWithValue("@Id", id);

            var affected = (int)(await command.ExecuteScalarAsync() ?? 0);
            return affected;
        }
    }
}
