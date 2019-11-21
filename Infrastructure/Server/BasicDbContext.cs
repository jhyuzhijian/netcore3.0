using Core_Entity.Entity;
using Infrastructure.Server.EntityTypeConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Infrastructure
{
    //public class entityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    //    where TEntity : class
    //{
    //    public void Configure(EntityTypeBuilder<TEntity> builder)
    //    {
    //        builder.ToTable("Role", "Right");
    //    }
    //}
    public class BasicDbContext : DbContext
    {
        public BasicDbContext(DbContextOptions<BasicDbContext> options) : base(options) { }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BaseEntityTypeConfiguration<Role>());
            modelBuilder.Entity<Group>().ToTable("Group", "User");
        }
        public override int SaveChanges()
        {
            try
            {
                ChangeTracker.DetectChanges();

                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
