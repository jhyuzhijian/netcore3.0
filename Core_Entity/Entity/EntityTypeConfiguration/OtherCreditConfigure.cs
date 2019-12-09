using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core_Entity.Entity.EntityTypeConfiguration
{
    public class OtherCreditConfigure : IEntityTypeConfiguration<OtherCredit>
    {
        public void Configure(EntityTypeBuilder<OtherCredit> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.TrainOtherCredit)
                .WithOne(o => o.OtherCredit)
                //.HasForeignKey<TrainOtherCredit>(p => p.OtherCreditId)
                ;
        }
    }
}
