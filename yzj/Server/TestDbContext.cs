using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using yzj.Model;

namespace yzj.Server
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions options) : base(options) { }
        public DbSet<TestModel> User_Login { get; set; }
    }
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
