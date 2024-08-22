using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddtableClientProfessional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientProfessionals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfessionalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DescriptionProfessional = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProfessionals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientProfessionals_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientProfessionals_Professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "Professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientProfessionals_ClientId",
                table: "ClientProfessionals",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProfessionals_ProfessionalId",
                table: "ClientProfessionals",
                column: "ProfessionalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientProfessionals");
        }
    }
}
