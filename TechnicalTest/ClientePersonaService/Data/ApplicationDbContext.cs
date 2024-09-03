using Microsoft.EntityFrameworkCore;
using ClientePersonaService.Models;

namespace ClientePersonaService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
    }
}
