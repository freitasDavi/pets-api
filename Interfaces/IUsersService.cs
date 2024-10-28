using Pets.DTOs.Requests.Auth;
using Pets.Models;

namespace Pets.Interfaces;

public interface IUsersService
{
    Task<IEnumerable<User>> GetUsers();
    Task AddUser(User user);
    Task UpdateUser(int id, User user);
    Task DeleteUser(int id);
    Task<string> Login(LoginDTO dto);
}