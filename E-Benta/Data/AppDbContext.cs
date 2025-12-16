using E_Benta.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Benta.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
    }
}
