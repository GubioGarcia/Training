using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Domain.Entities;
using Training.Domain.Models;

namespace Training.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder ApplyGlobalConfiguration(this ModelBuilder builder)
        {
            foreach (IMutableEntityType entityType in builder.Model.GetEntityTypes())
            {
                foreach (IMutableProperty property in entityType.GetProperties())
                {
                    switch (property.Name)
                    {
                        case nameof(EntityUsers.Id):
                            property.IsKey();   
                            break;
                        case nameof(EntityUsers.DateUpdated):
                            property.IsNullable = true;
                            break;
                        case nameof(EntityUsers.IsDeleted):
                            property.IsNullable = false;
                            property.SetDefaultValue(false);
                            break;
                        case nameof(EntityUsers.IsActive):
                            property.IsNullable = false;
                            property.SetDefaultValue(true);
                            break;
                        case nameof(EntityUsers.DateRegistration):
                            property.IsNullable = false;
                            break;
                        default:
                            break;
                    }
                }
            }

            return builder;
        }

        public static ModelBuilder SeedData(this ModelBuilder builder)
        {
            builder.Entity<UsersType>()
                .HasData(
                    new UsersType 
                    {
                        Id = Guid.Parse("c4b3e3a7-2f0b-4e6e-9f5e-8e2a1e1d8a4b"),
                        Name = "Admin",
                        Description = "Usuário gerenciador do sistema. Possui acesso a manipulação de dados gerais e dados referente a usuários do tipo 'Professional'.",
                        AccessLevel = 0
                    }
                );

            builder.Entity<UsersType>()
                .HasData(
                    new UsersType
                    {
                        Id = Guid.Parse("a8f6c1e1-9d5e-4a2d-8c6f-7b3e0f9d6a6e"),
                        Name = "Professional",
                        Description = "Usuário gerenciador de usuários do tipo 'Client'.",
                        AccessLevel = 1
                    }
                );

            builder.Entity<UsersType>()
                .HasData(
                    new UsersType
                    {
                        Id = Guid.Parse("b7d8f9e0-3c4a-4b6e-9d1f-2e5c6a7b8f0d"),
                        Name = "Client",
                        Description = "Usuário final da plataforma. Possui gestão a suas próprias informações de perfil e registro de atividades.",
                        AccessLevel = 2
                    }
                );

            builder.Entity<ProfessionalType>()
                .HasData(
                    new ProfessionalType
                    {
                        Id = Guid.Parse("e2a1b0c9-8d7e-6f5a-4b3c-1e9d0c2b5a8f"),
                        Name = "Admin",
                        Description = "Usuário gerenciador do sistema. Possui acesso a manipulação de dados gerais e dados referente a usuários do tipo 'Professional'."
                    }
                );

            builder.Entity<ProfessionalType>()
                .HasData(
                    new ProfessionalType
                    {
                        Id = Guid.Parse("4e5d6c7b-1a2f-3e8d-9b0c-5a6f7d8e2c1b"),
                        Name = "Personal",
                        Description = "Profissional da área de educação física. Possui acesso a manipulação de clientes, montagem de protocolos de treino e registros do cliente."
                    }
                );

            builder.Entity<Professional>()
                .HasData(
                    new Professional
                    {
                        Id = Guid.Parse("f0e1d2c3-5b6a-7d8e-9f0c-1a2b3e4d5c6f"),
                        UsersTypeId = Guid.Parse("c4b3e3a7-2f0b-4e6e-9f5e-8e2a1e1d8a4b"),
                        ProfessionalTypesId = Guid.Parse("e2a1b0c9-8d7e-6f5a-4b3c-1e9d0c2b5a8f"),
                        ProfessionalRegistration = "Admin",
                        IsActive = true,
                        Cpf = "30704958341",
                        Password = "$2a$11$e34Jq7J0U4e8FT6DjnQilOXYWvKtx7iIz5jSid5YMwHsxO.pbNgTS",
                        Name = "Professional Default Admin",
                        DateRegistration = new DateTime(2024,07,21),
                        Fone = "5562999999999",
                        CurrentNumberClients = 0,
                        DateUpdated = null,
                        IsDeleted = false
                    }
                );

            return builder;
        }
    }
}
