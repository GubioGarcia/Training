using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Training.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initialcreatetables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProfessionalTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionalTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AccessLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InitialObjective = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Heigth = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    StartingWeight = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    CurrentWeight = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateRegistration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fone = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    UrlProfilePhoto = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_UsersTypes_UsersTypeId",
                        column: x => x.UsersTypeId,
                        principalTable: "UsersTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Professionals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfessionalTypesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfessionalRegistration = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CurrentNumberClients = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateRegistration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fone = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    UrlProfilePhoto = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professionals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Professionals_ProfessionalTypes_ProfessionalTypesId",
                        column: x => x.ProfessionalTypesId,
                        principalTable: "ProfessionalTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Professionals_UsersTypes_UsersTypeId",
                        column: x => x.UsersTypeId,
                        principalTable: "UsersTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ProfessionalTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("4e5d6c7b-1a2f-3e8d-9b0c-5a6f7d8e2c1b"), "Profissional da área de educação física. Possui acesso a manipulação de clientes, montagem de protocolos de treino e registros do cliente.", "Personal" },
                    { new Guid("e2a1b0c9-8d7e-6f5a-4b3c-1e9d0c2b5a8f"), "Usuário gerenciador do sistema. Possui acesso a manipulação de dados gerais e dados referente a usuários do tipo 'Professional'.", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "UsersTypes",
                columns: new[] { "Id", "AccessLevel", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("a8f6c1e1-9d5e-4a2d-8c6f-7b3e0f9d6a6e"), 1, "Usuário gerenciador de usuários do tipo 'Client'.", "Professional" },
                    { new Guid("b7d8f9e0-3c4a-4b6e-9d1f-2e5c6a7b8f0d"), 2, "Usuário final da plataforma. Possui gestão a suas próprias informações de perfil e registro de atividades.", "Client" },
                    { new Guid("c4b3e3a7-2f0b-4e6e-9f5e-8e2a1e1d8a4b"), 0, "Usuário gerenciador do sistema. Possui acesso a manipulação de dados gerais e dados referente a usuários do tipo 'Professional'.", "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Professionals",
                columns: new[] { "Id", "Cpf", "CurrentNumberClients", "DateRegistration", "Fone", "IsActive", "Name", "ProfessionalRegistration", "ProfessionalTypesId", "UrlProfilePhoto", "UsersTypeId" },
                values: new object[] { new Guid("f0e1d2c3-5b6a-7d8e-9f0c-1a2b3e4d5c6f"), "30704958341", 0, new DateTime(2024, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "5562999999999", true, "Professional Default Admin", "Admin", new Guid("e2a1b0c9-8d7e-6f5a-4b3c-1e9d0c2b5a8f"), null, new Guid("c4b3e3a7-2f0b-4e6e-9f5e-8e2a1e1d8a4b") });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_UsersTypeId",
                table: "Clients",
                column: "UsersTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_ProfessionalTypesId",
                table: "Professionals",
                column: "ProfessionalTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_UsersTypeId",
                table: "Professionals",
                column: "UsersTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Professionals");

            migrationBuilder.DropTable(
                name: "ProfessionalTypes");

            migrationBuilder.DropTable(
                name: "UsersTypes");
        }
    }
}
