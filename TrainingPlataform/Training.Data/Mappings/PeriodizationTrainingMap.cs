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
            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);
            builder.Property(x => x.DateStart).IsRequired();
            builder.Property(x => x.DateEnd).IsRequired();
            builder.Property(x => x.WeeklyTrainingFrequency).IsRequired();
            builder.Property(x => x.DateUpdated).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);

            builder.HasOne(x => x.Professional)
                   .WithMany(p => p.PeriodizationTrainings)
                   .HasForeignKey(x => x.ProfessionalId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Client)
                   .WithMany(p => p.PeriodizationTrainings)
                   .HasForeignKey(x => x.ClientId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
