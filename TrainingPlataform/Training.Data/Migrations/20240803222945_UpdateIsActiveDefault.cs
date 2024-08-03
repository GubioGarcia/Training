using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIsActiveDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateRegistration",
                table: "Professionals",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 8, 3, 17, 25, 10, 887, DateTimeKind.Local).AddTicks(7982));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateRegistration",
                table: "Clients",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 8, 3, 17, 25, 10, 887, DateTimeKind.Local).AddTicks(7197));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateRegistration",
                table: "Professionals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 3, 17, 25, 10, 887, DateTimeKind.Local).AddTicks(7982),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateRegistration",
                table: "Clients",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 3, 17, 25, 10, 887, DateTimeKind.Local).AddTicks(7197),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
