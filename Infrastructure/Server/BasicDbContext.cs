using Core_Entity;
using Core_Entity.Entity;
using Infrastructure.Server;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Infrastructure
{
    //用仓储的方式去代替声明DBSet<T>,省的新加表要加的东西那么多
    public class BasicDbContext : DbContext
    {
        public BasicDbContext(DbContextOptions<BasicDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        //public DbSet<Group> Groups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder
            //   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            //   ;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //EF上下文
            modelBuilder.ExecuteConfigurations();

            base.OnModelCreating(modelBuilder);

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
