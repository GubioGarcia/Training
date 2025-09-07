using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Domain.Entities;

namespace Training.Data.Mappings
{
    public class PeriodizationTrainingMap : IEntityTypeConfiguration<PeriodizationTraining>
    {
        public void Configure(EntityTypeBuilder<PeriodizationTraining> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DateUpdated).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);

            builder.HasOne(x => x.Periodization)
                   .WithMany(p => p.PeriodizationTrainings)
                   .HasForeignKey(x => x.PeriodizationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Training)
                   .WithMany(t => t.PeriodizationTrainings)
                   .HasForeignKey(x => x.TrainingId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
