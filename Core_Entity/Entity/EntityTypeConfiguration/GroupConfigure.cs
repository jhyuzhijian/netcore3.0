using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core_Entity.Entity.EntityTypeConfiguration
{
    public class GroupConfigure : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey("Id");
            //#region 并发处理
            ////设置版本（并发操作表数据时，会报错，触发DbUpdateConcurrencyException异常）
            //builder.Property(e => e.timeSpan).IsRowVersion();
            //#endregion
        }
    }
}
