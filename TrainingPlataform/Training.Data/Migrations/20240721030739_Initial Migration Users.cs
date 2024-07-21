using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrationUsers : Migration
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
                    UsersType_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                        name: "FK_Clients_UsersTypes_UsersType_Id",
                        column: x => x.UsersType_Id,
                        principalTable: "UsersTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Professionals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersType_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfessionalTypes_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfessionalTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfessionalRegistration = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                        name: "FK_Professionals_ProfessionalTypes_ProfessionalTypeId",
                        column: x => x.ProfessionalTypeId,
                        principalTable: "ProfessionalTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Professionals_UsersTypes_UsersType_Id",
                        column: x => x.UsersType_Id,
                        principalTable: "UsersTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_UsersType_Id",
                table: "Clients",
                column: "UsersType_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_ProfessionalTypeId",
                table: "Professionals",
                column: "ProfessionalTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_UsersType_Id",
                table: "Professionals",
                column: "UsersType_Id");
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
