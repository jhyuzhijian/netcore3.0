using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core_Entity.Entity.EntityTypeConfiguration
{
    public class RelationExtendConfigure : IEntityTypeConfiguration<RelationExtend>
    {
        public void Configure(EntityTypeBuilder<RelationExtend> builder)
        {
            builder.ToTable("RelationExtend", "Summary")
                .HasKey(p => p.Id)
                ;
        }
    }
}
