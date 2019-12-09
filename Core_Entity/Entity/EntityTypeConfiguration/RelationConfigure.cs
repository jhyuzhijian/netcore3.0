using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core_Entity.Entity.EntityTypeConfiguration
{
    public class RelationConfigure : IEntityTypeConfiguration<Relation>
    {
        public void Configure(EntityTypeBuilder<Relation> builder)
        {
            builder.HasKey(p => p.Id)
               ;
            //一对一的关系
            builder.ToTable("Relation", "Summary")
                .HasOne(p => p.RelationExtend)
                .WithOne(p => p.Relation)
                .HasForeignKey<RelationExtend>(p => p.RelationId)
                ;


        }
    }
}
