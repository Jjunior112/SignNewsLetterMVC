using Microsoft.EntityFrameworkCore;

namespace SignNewsLetter.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Subscribe> Subscribes { get; set; }
    }
}