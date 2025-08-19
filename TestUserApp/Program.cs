using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using TestUserApp;
using TestUserApp.Models;
using TestUserApp.Repositories;
using TestUserApp.Repositories.Interfaces;
using TestUserApp.Services;
using TestUserApp.Services.Interfaces;

var connectionString = "Server=localhost;Database=TestDatabase;Trusted_Connection=True;TrustServerCertificate=True;";
var factory = new SqlConnectionFactory(connectionString);

IUserRepository repo = new UserRepository(factory);
IUserService service = new UserService(repo);

Console.WriteLine("q - quit");
while (true)
{
    Console.WriteLine("Choose option: \n1 - List,\n2 - Get user by id, \n3 - Add user, \n4 - Update user, \n5 - Delete user");
    var option = Console.ReadLine();

    if (option?.ToLower() == "q") break;

    try
    {
        switch (option)
        {
            case "1":
                {
                    var users = await service.GetAllAsync();
                    if (users.Count == 0)
                    {
                        Console.WriteLine("There are no users in this database yet");
                        break;
                    }
                    foreach (var user in users)
                    {
                        Console.WriteLine(
                            $"{user.Id}: {user.Name} {user.Surname ?? ""} Age={user.Age} Birth date={user.BirthDate?.ToString() ?? ""}");
                    }
                    break;
                }
            case "2":
                {
                    Console.Write("Id: ");
                    int.TryParse(Console.ReadLine(), out var id);
                    var user = await service.GetUserByIdAsync(id);
                    Console.WriteLine(user is null ? $"User with id {id} not found" :
                        $"{user.Id}: {user.Name} {user.Surname ?? ""} Age={user.Age} Birth date={user.BirthDate?.ToString() ?? ""}");
                    break;
                }
            case "3":
                {
                    Console.Write("Name: ");
                    var name = Console.ReadLine() ?? "";
                    Console.Write("Surname: ");
                    var surname = Console.ReadLine() ?? null;
                    Console.Write("Age: ");
                    int.TryParse(Console.ReadLine(), out var age);
                    Console.Write("Birth date: ");
                    DateOnly? dateOfBirth = DateOnly.TryParse(Console.ReadLine(), out var date) ? date : null;
                    var newId = await service.CreateUserAsync(name, surname, age, dateOfBirth);
                    Console.WriteLine($"Created Id={newId}");
                    break;
                }
            case "4":
                {
                    Console.Write("Id: ");
                    int.TryParse(Console.ReadLine(), out var id);
                    Console.Write("Name: ");
                    var name = Console.ReadLine() ?? "";
                    Console.Write("Surname: ");
                    var surname = Console.ReadLine() ?? null;
                    Console.Write("Age: ");
                    int.TryParse(Console.ReadLine(), out var age);
                    Console.Write("Birth date: ");
                    DateOnly? dateOfBirth = DateOnly.TryParse(Console.ReadLine(), out var date) ? date : null;
                    var updated = await service.UpdateUserAsync(id, name, surname, age, dateOfBirth);
                    Console.WriteLine($"Updated: {updated}");
                    break;
                }
            case "5":
                {
                    Console.Write("Id: ");
                    int.TryParse(Console.ReadLine(), out var id);
                    var deleted = await service.DeleteUserAsync(id);
                    Console.WriteLine($"Deleted: {deleted}");
                    break;
                }
            default:
                Console.WriteLine("Unknown command.");
                break;
        }
    } 
    catch (SqlException ex)
    {
        Console.WriteLine($"[SQL] {ex.Number}: {ex.Message}");
    }
    catch (Exception ex) 
    { 
        Console.WriteLine($"[ERR] {ex.Message}"); 
    }
}

