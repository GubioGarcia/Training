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
    public class ClientMap : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.Cpf).IsRequired().HasMaxLength(11);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.DateRegistration).IsRequired();
            builder.Property(x => x.Fone).IsRequired().HasMaxLength(13);
            builder.Property(x => x.UrlProfilePhoto).HasMaxLength(255);
            builder.Property(x => x.DateBirth).IsRequired();
            builder.Property(x => x.InitialObjective).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Heigth).IsRequired().HasColumnType("decimal(3,2)");
            builder.Property(x => x.StartingWeight).IsRequired().HasColumnType("decimal(5,2)");
            builder.Property(x => x.CurrentWeight).IsRequired().HasColumnType("decimal(5,2)");

            builder.HasOne(x => x.UsersType)
                   .WithMany(u => u.Clients)
                   .HasForeignKey(x => x.UsersTypeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}