using collection_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace collection_backend.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<Tool> Tools { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    }
}
