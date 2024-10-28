using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pets.DTOs.Requests.Auth;
using Pets.Interfaces;
using Pets.Models;
using Pets.Utils;

namespace Pets.Services;

public class UsersService : IUsersService
{
    private readonly Context _context;
    private readonly IConfiguration _configuration;

    public UsersService(Context context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task AddUser(User user)
    {
        var hashPassword = HashPassword(user.Password); 
        
        user.Password = hashPassword;
        
        await _context.Users.AddAsync(user);
        
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateUser(int id, User user)
    {
        var userFound = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (userFound == null) throw new Exception("User not found");
        
        _context.Users.Update(user);
            
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(int id)
    {
        var userFound = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (userFound == null) throw new Exception("User not found");
        
        _context.Users.Remove(userFound);
            
        await _context.SaveChangesAsync();
    }

    public async Task<string> Login(LoginDTO dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        
        if (user == null) throw new Exception("Email or password is incorrect");

        if (!ComparePassword(dto.Password, user.Password)) throw new Exception("Email or password is incorrect");
        
        return GenerateJsonWebtoken(user);
    }

    private string GenerateJsonWebtoken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Sid, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name)
        };
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Token").GetValue<string>("Key")!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            claims: claims,
            issuer: _configuration.GetSection("Token").GetValue<string>("Issuer"),
            audience: _configuration.GetSection("Token").GetValue<string>("Audience"),
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool ComparePassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
    
}