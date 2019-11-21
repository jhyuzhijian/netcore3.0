using Core_Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Server.EntityTypeConfiguration
{
    /// <summary>
    /// schema默认为dbo
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
         where TEntity : class, IEntity
    {
        private string _tableName { get; }
        private string _schema { get; }
        public BaseEntityTypeConfiguration() { }
        /// <summary>
        /// TEntity未指定System.ComponentModel.DataAnnotations.Schema.TableAttribute时需指定构造参数
        /// </summary>
        /// <param name="tableName">数据库的表名</param>
        /// <param name="schema">数据库的组织架构</param>
        public BaseEntityTypeConfiguration(string tableName, string schema = "dbo")
        {
            _tableName = tableName;
            _schema = schema;
        }
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            if (!string.IsNullOrEmpty(_tableName) && !string.IsNullOrEmpty(_schema))
                builder.ToTable(_tableName, _schema);
        }
    }
}
