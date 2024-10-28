using Microsoft.EntityFrameworkCore;
using Pets.Models;

namespace Pets.Utils;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options) 
    {
    }
    
    public DbSet<User> Users { get; set; }
}