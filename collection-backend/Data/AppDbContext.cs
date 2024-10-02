using collection_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace collection_backend.Data
{
    public class AppDbContext: DbContext
    {
        DbSet<Tool> Tools;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    }
}
