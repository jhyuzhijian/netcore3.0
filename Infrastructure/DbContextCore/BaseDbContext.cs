using Core_Entity;
using Core_Entity.Entity;
using Infrastructure.DBContextCore;
using Infrastructure.DbContextLog;
using Infrastructure.IDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.DbContextCore
{
    //EFCore3.0不支持lazy加载
    //用仓储的方式去代替声明DBSet<T>,省的新加表要加的东西那么多
    public abstract class BaseDbContext : DbContext, IDbContextCore
    {
        public DatabaseFacade GetDatabase() => base.Database;
        public BaseDbContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
            //关闭跟踪,对应detached的状态
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        }
        #region 简易的logfactory
        public static readonly ILoggerFactory MyLoggerFactory
    = LoggerFactory.Create(builder =>
    {
        builder
            .AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name
                && level == LogLevel.Information)
            .AddConsole();
    });
        #endregion
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #region 所有EFCore查询语句都输出到日志中
            //optionsBuilder
            //    .UseLoggerFactory(new EFCoreFactory())
            //    ;
            //optionsBuilder.EnableSensitiveDataLogging();
            #endregion
            //optionsBuilder.UseDatabaseNullSemantics
            base.OnConfiguring(optionsBuilder);

            //optionsBuilder
            //   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            //   ;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //EF上下文
            modelBuilder.ExecuteConfigurations();
            //modelBuilder.Entity<>().ToTable
            //modelBuilder.ApplyConfiguration(new Core_Entity.Entity.EntityTypeConfiguration.OtherCreditConfigure());
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
        #region 操作数据库
        public int Add<T>(T entity) where T : class
        {
            base.Add(entity);
            return SaveChanges();
        }
        public Task<int> AddAsync<T>(T entity) where T : class
        {
            base.Add(entity);
            return SaveChangesAsync();
        }
        public virtual int AddRange<T>(ICollection<T> entities) where T : class
        {
            base.AddRange(entities);
            return SaveChanges();
        }
        public virtual Task<int> AddRangeAsync<T>(ICollection<T> entities) where T : class
        {
            base.AddRangeAsync(entities);
            return SaveChangesAsync();
        }
        public virtual T Find<T>(object key) where T : class
        {
            return base.Find<T>(key);
        }
        public virtual ValueTask<T> FindAsync<T>(object key) where T : class
        {
            return base.FindAsync<T>(key);
        }
        public virtual IQueryable<T> Get<T>(Expression<Func<T, bool>> @where = null, bool asNoTracking = false) where T : class
        {
            var query = GetDbSet<T>().AsQueryable();
            if (where != null)
                query = query.Where(where);
            if (asNoTracking)
                query = query.AsNoTracking();
            return query;
        }
        public virtual List<IEntityType> GetAllEntityTypes()
        {
            return Model.GetEntityTypes().ToList();
        }
        public virtual DbSet<T> GetDbSet<T>()
            where T : class
        {
            if (Model.FindEntityType(typeof(T)) != null)
            {
                return Set<T>();
            }
            throw new Exception(string.Format("类型{0}未注册到在数据库上下文中,请继承IEntity完成注册", typeof(T).Name));
        }
        public virtual int Count<T>(Expression<Func<T, bool>> @where = null)
            where T : class
        {
            return where == null ? GetDbSet<T>().Count() :
                GetDbSet<T>().Where(where).Count();
        }
        public virtual Task<int> CountAsync<T>(Expression<Func<T, bool>> @where = null) where T : class
        {
            return (where == null ? GetDbSet<T>().CountAsync() : GetDbSet<T>().CountAsync(@where));
        }
        public virtual int Delete<T>(object key) where T : class
        {
            var entity = Find<T>(key);
            Remove(entity);
            return SaveChanges();
        }
        public virtual Task<int> DeleteAsync<T>(object key) where T : class
        {
            var entity = Find<T>(key);
            Remove(entity);
            return SaveChangesAsync();
        }
        public virtual int ExecuteSqlWithNonQuery(string sql, params object[] paramters)
        {
            return Database.ExecuteSqlRaw(sql, CancellationToken.None, paramters);
        }
        public virtual Task<int> ExecuteSqlWithNonQueryAsync(string sql, params object[] paramters)
        {
            return Database.ExecuteSqlRawAsync(sql, CancellationToken.None, paramters);
        }
        /// <summary>
        /// 修改表内所有字段(修改部分字段需要用attch,entitys则使用AttachRange)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual int UpdateEntity<T>(T entity) where T : class
        {
            base.Update(entity);
            base.Entry(entity).State = EntityState.Modified;
            return SaveChanges();
        }
        /// <summary>
        /// 修改表内所有字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual Task<int> UpdateEntityList<T>(System.Collections.Generic.ICollection<T> entities) where T : class
        {
            GetDbSet<T>().UpdateRange(entities.ToArray());

            return SaveChangesAsync();
        }
        public virtual bool Exist<T>(Expression<Func<T, bool>> @where = null) where T : class
        {
            return @where == null ? GetDbSet<T>().Any() : GetDbSet<T>().Any(@where);
        }
        public virtual Task<bool> ExistAsync<T>(Expression<Func<T, bool>> @where = null) where T : class
        {
            return (@where == null ? GetDbSet<T>().AnyAsync() : GetDbSet<T>().AnyAsync(@where));
        }
        public virtual IQueryable<T> FilterWithInclude<T>(Func<IQueryable<T>, IQueryable<T>> include, Expression<Func<T, bool>> @where) where T : class
        {
            var result = GetDbSet<T>().AsQueryable();
            if (where != null)
                result = GetDbSet<T>().Where(where);
            if (include != null)
                result = include(result);
            return result;
        }
        public virtual T GetSingleOrDefault<T>(Expression<Func<T, bool>> @where = null) where T : class
        {
            return where == null ? GetDbSet<T>().SingleOrDefault() : GetDbSet<T>().SingleOrDefault(where);
        }

        public virtual Task<T> GetSingleOrDefaultAsync<T>(Expression<Func<T, bool>> @where = null) where T : class
        {
            return where == null ? GetDbSet<T>().SingleOrDefaultAsync() : GetDbSet<T>().SingleOrDefaultAsync(where);
        }
        public virtual int Update<T>(T model, params string[] updateColumns) where T : class
        {
            if (updateColumns != null && updateColumns.Length > 0)
            {
                if (Entry(model).State == EntityState.Added ||
                    Entry(model).State == EntityState.Detached) GetDbSet<T>().Attach(model);
                foreach (var propertyName in updateColumns)
                {
                    Entry(model).Property(propertyName).IsModified = true;
                }
            }
            else
            {
                Entry(model).State = EntityState.Modified;
            }
            return SaveChanges();
        }
        public virtual int Update<T>(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateFactory) where T : class
        {
            var list = GetDbSet<T>();
            GetDbSet<T>().UpdateRange(list);

            return SaveChanges();
        }
        public virtual Task<int> UpdateAsync<T>(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateFactory) where T : class
        {
            var list = GetDbSet<T>();
            GetDbSet<T>().UpdateRange(list);
            return SaveChangesAsync();
            //return await GetDbSet<T>().Where(where).UpdateAsync(updateFactory);
        }

        /// <summary>
        /// delete by query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual int Delete<T>(Expression<Func<T, bool>> @where) where T : class
        {
            var dbSet = GetDbSet<T>();
            dbSet.RemoveRange(dbSet.Where(@where));

            return SaveChanges(); ;
        }

        public virtual Task<int> DeleteAsync<T>(Expression<Func<T, bool>> @where) where T : class
        {
            var dbSet = GetDbSet<T>();
            dbSet.RemoveRange(dbSet.Where(@where));
            return SaveChangesAsync();
        }
        public virtual List<TView> SqlQuery<T, TView>(string sql, params object[] parameters)
           where T : class
        {
            return GetDbSet<T>().FromSqlRaw(sql, parameters).Cast<TView>().ToList();
        }
        public virtual Task<List<TView>> SqlQueryAsync<T, TView>(string sql, params object[] parameters)
           where T : class
           where TView : class
        {
            return GetDbSet<T>().FromSqlRaw(sql, parameters).Cast<TView>().ToListAsync();
        }
        /// <summary>
        /// bulk insert by sqlbulkcopy, and with transaction.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entities"></param>
        /// <param name="destinationTableName"></param>
        public virtual void BulkInsert<T>(IList<T> entities, string destinationTableName = null) where T : class
        {
            if (!Database.IsSqlServer())
                throw new NotSupportedException("This method only supports for SQL Server");
        }
        public abstract DataTable GetDataTable(string sql, params DbParameter[] parameters);
        public abstract List<DataTable> GetDataTables(string sql, params DbParameter[] parameters);
        #endregion
    }
}
