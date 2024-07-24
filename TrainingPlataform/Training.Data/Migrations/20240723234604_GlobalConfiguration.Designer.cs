﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Training.Data.Context;

#nullable disable

namespace Training.Data.Migrations
{
    [DbContext(typeof(TrainingContext))]
    [Migration("20240723234604_GlobalConfiguration")]
    partial class GlobalConfiguration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Training.Domain.Entities.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<decimal>("CurrentWeight")
                        .HasColumnType("decimal(5,2)");

                    b.Property<DateTime>("DateBirth")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateRegistration")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Fone")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<decimal>("Heigth")
                        .HasColumnType("decimal(3,2)");

                    b.Property<string>("InitialObjective")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("StartingWeight")
                        .HasColumnType("decimal(5,2)");

                    b.Property<string>("UrlProfilePhoto")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid>("UsersTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UsersTypeId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Training.Domain.Entities.Professional", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<int>("CurrentNumberClients")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateRegistration")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Fone")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ProfessionalRegistration")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("ProfessionalTypesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UrlProfilePhoto")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid>("UsersTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProfessionalTypesId");

                    b.HasIndex("UsersTypeId");

                    b.ToTable("Professionals");

                    b.HasData(
                        new
                        {
                            Id = new Guid("f0e1d2c3-5b6a-7d8e-9f0c-1a2b3e4d5c6f"),
                            Cpf = "30704958341",
                            CurrentNumberClients = 0,
                            DateRegistration = new DateTime(2024, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Fone = "5562999999999",
                            IsActive = true,
                            IsDeleted = false,
                            Name = "Professional Default Admin",
                            ProfessionalRegistration = "Admin",
                            ProfessionalTypesId = new Guid("e2a1b0c9-8d7e-6f5a-4b3c-1e9d0c2b5a8f"),
                            UsersTypeId = new Guid("c4b3e3a7-2f0b-4e6e-9f5e-8e2a1e1d8a4b")
                        });
                });

            modelBuilder.Entity("Training.Domain.Entities.ProfessionalType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ProfessionalTypes");

                    b.HasData(
                        new
                        {
                            Id = new Guid("e2a1b0c9-8d7e-6f5a-4b3c-1e9d0c2b5a8f"),
                            Description = "Usuário gerenciador do sistema. Possui acesso a manipulação de dados gerais e dados referente a usuários do tipo 'Professional'.",
                            IsDeleted = false,
                            Name = "Admin"
                        },
                        new
                        {
                            Id = new Guid("4e5d6c7b-1a2f-3e8d-9b0c-5a6f7d8e2c1b"),
                            Description = "Profissional da área de educação física. Possui acesso a manipulação de clientes, montagem de protocolos de treino e registros do cliente.",
                            IsDeleted = false,
                            Name = "Personal"
                        });
                });

            modelBuilder.Entity("Training.Domain.Entities.UsersType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessLevel")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("UsersTypes");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c4b3e3a7-2f0b-4e6e-9f5e-8e2a1e1d8a4b"),
                            AccessLevel = 0,
                            Description = "Usuário gerenciador do sistema. Possui acesso a manipulação de dados gerais e dados referente a usuários do tipo 'Professional'.",
                            IsDeleted = false,
                            Name = "Admin"
                        },
                        new
                        {
                            Id = new Guid("a8f6c1e1-9d5e-4a2d-8c6f-7b3e0f9d6a6e"),
                            AccessLevel = 1,
                            Description = "Usuário gerenciador de usuários do tipo 'Client'.",
                            IsDeleted = false,
                            Name = "Professional"
                        },
                        new
                        {
                            Id = new Guid("b7d8f9e0-3c4a-4b6e-9d1f-2e5c6a7b8f0d"),
                            AccessLevel = 2,
                            Description = "Usuário final da plataforma. Possui gestão a suas próprias informações de perfil e registro de atividades.",
                            IsDeleted = false,
                            Name = "Client"
                        });
                });

            modelBuilder.Entity("Training.Domain.Entities.Client", b =>
                {
                    b.HasOne("Training.Domain.Entities.UsersType", "UsersType")
                        .WithMany("Clients")
                        .HasForeignKey("UsersTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("UsersType");
                });

            modelBuilder.Entity("Training.Domain.Entities.Professional", b =>
                {
                    b.HasOne("Training.Domain.Entities.ProfessionalType", "ProfessionalType")
                        .WithMany("Professionals")
                        .HasForeignKey("ProfessionalTypesId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Training.Domain.Entities.UsersType", "UsersType")
                        .WithMany("Professionals")
                        .HasForeignKey("UsersTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ProfessionalType");

                    b.Navigation("UsersType");
                });

            modelBuilder.Entity("Training.Domain.Entities.ProfessionalType", b =>
                {
                    b.Navigation("Professionals");
                });

            modelBuilder.Entity("Training.Domain.Entities.UsersType", b =>
                {
                    b.Navigation("Clients");

                    b.Navigation("Professionals");
                });
#pragma warning restore 612, 618
        }
    }
}