using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training.Data.Migrations
{
    /// <inheritdoc />
    public partial class PasswordField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Professionals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Professionals",
                keyColumn: "Id",
                keyValue: new Guid("f0e1d2c3-5b6a-7d8e-9f0c-1a2b3e4d5c6f"),
                column: "Password",
                value: "$2a$12$X7E9OQ9y9.H7rYVcOGBZQ.uoP.eNgpVpDw2G9hI9T7upICGq8kgy6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Clients");
        }
    }
}
