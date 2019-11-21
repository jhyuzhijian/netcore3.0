using Core_Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class BasicDbContext : DbContext
    {
        public BasicDbContext(DbContextOptions<BasicDbContext> options) : base(options) { }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().ToTable("Group", "User");
        }
    }
}
