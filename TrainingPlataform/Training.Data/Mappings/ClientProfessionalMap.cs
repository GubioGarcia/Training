using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Domain.Entities;

namespace Training.Data.Mappings
{
    public class ClientProfessionalMap : IEntityTypeConfiguration<ClientProfessional>
    {
        public void Configure(EntityTypeBuilder<ClientProfessional> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.DescriptionProfessional).IsRequired().HasMaxLength(500);

            builder.HasOne(x => x.Professional)
                   .WithMany(p => p.ClientProfessionals)
                   .HasForeignKey(x => x.ProfessionalId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Client)
                   .WithMany(c => c.ClientProfessionals)
                   .HasForeignKey(x => x.ClientId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
