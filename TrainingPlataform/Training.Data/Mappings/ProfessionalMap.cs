using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Training.Domain.Entities;

namespace Training.Data.Mappings
{
    public class ProfessionalMap : IEntityTypeConfiguration<Professional>
    {
        public void Configure(EntityTypeBuilder<Professional> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(x => x.ProfessionalRegistration).HasMaxLength(50);
            builder.Property(x => x.Cpf).IsRequired().HasMaxLength(11);
            builder.Property(x => x.Password).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.DateRegistration).IsRequired();
            builder.Property(x => x.Fone).IsRequired().HasMaxLength(13);
            builder.Property(x => x.CurrentNumberClients).IsRequired();
            builder.Property(x => x.UrlProfilePhoto).HasMaxLength(255);

            builder.HasOne(x => x.UsersType)
                   .WithMany(p => p.Professionals)
                   .HasForeignKey(x => x.UsersTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ProfessionalType)
                   .WithMany(p => p.Professionals)
                   .HasForeignKey(x => x.ProfessionalTypesId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
