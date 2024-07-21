using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training.Data.Migrations
{
    /// <inheritdoc />
    public partial class CommonFieldsv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "UsersTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UsersTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "ProfessionalTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProfessionalTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "ProfessionalTypes",
                keyColumn: "Id",
                keyValue: new Guid("4e5d6c7b-1a2f-3e8d-9b0c-5a6f7d8e2c1b"),
                columns: new[] { "DateUpdated", "IsDeleted" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "ProfessionalTypes",
                keyColumn: "Id",
                keyValue: new Guid("e2a1b0c9-8d7e-6f5a-4b3c-1e9d0c2b5a8f"),
                columns: new[] { "DateUpdated", "IsDeleted" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "UsersTypes",
                keyColumn: "Id",
                keyValue: new Guid("a8f6c1e1-9d5e-4a2d-8c6f-7b3e0f9d6a6e"),
                columns: new[] { "DateUpdated", "IsDeleted" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "UsersTypes",
                keyColumn: "Id",
                keyValue: new Guid("b7d8f9e0-3c4a-4b6e-9d1f-2e5c6a7b8f0d"),
                columns: new[] { "DateUpdated", "IsDeleted" },
                values: new object[] { null, false });

            migrationBuilder.UpdateData(
                table: "UsersTypes",
                keyColumn: "Id",
                keyValue: new Guid("c4b3e3a7-2f0b-4e6e-9f5e-8e2a1e1d8a4b"),
                columns: new[] { "DateUpdated", "IsDeleted" },
                values: new object[] { null, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "UsersTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UsersTypes");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "ProfessionalTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProfessionalTypes");
        }
    }
}
