using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePeriodizationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_Professionals_ProfessionalId",
                table: "Exercise");

            migrationBuilder.DropForeignKey(
                name: "FK_MuscleGroup_Professionals_ProfessionalId",
                table: "MuscleGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_PeriodizationTraining_Clients_ClientId",
                table: "PeriodizationTraining");

            migrationBuilder.DropForeignKey(
                name: "FK_PeriodizationTraining_Professionals_ProfessionalId",
                table: "PeriodizationTraining");

            migrationBuilder.DropForeignKey(
                name: "FK_Training_PeriodizationTraining_PeriodizationTrainingId",
                table: "Training");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutCategory_Professionals_ProfessionalId",
                table: "WorkoutCategory");

            migrationBuilder.DropIndex(
                name: "IX_Training_PeriodizationTrainingId",
                table: "Training");

            migrationBuilder.DropColumn(
                name: "PeriodizationTrainingId",
                table: "Training");

            migrationBuilder.DropColumn(
                name: "DateEnd",
                table: "PeriodizationTraining");

            migrationBuilder.DropColumn(
                name: "DateStart",
                table: "PeriodizationTraining");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PeriodizationTraining");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PeriodizationTraining");

            migrationBuilder.DropColumn(
                name: "WeeklyTrainingFrequency",
                table: "PeriodizationTraining");

            migrationBuilder.RenameColumn(
                name: "ProfessionalId",
                table: "PeriodizationTraining",
                newName: "TrainingId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "PeriodizationTraining",
                newName: "PeriodizationId");

            migrationBuilder.RenameIndex(
                name: "IX_PeriodizationTraining_ProfessionalId",
                table: "PeriodizationTraining",
                newName: "IX_PeriodizationTraining_TrainingId");

            migrationBuilder.RenameIndex(
                name: "IX_PeriodizationTraining_ClientId",
                table: "PeriodizationTraining",
                newName: "IX_PeriodizationTraining_PeriodizationId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WorkoutCategory",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "WorkoutCategory",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "WorkoutCategory",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Training",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Training",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Training",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProfessionalTypes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ProfessionalTypes",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "PeriodizationTraining",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MuscleGroup",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MuscleGroup",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "MuscleGroup",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Exercise",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Exercise",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Exercise",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Periodization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfessionalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WeeklyTrainingFrequency = table.Column<int>(type: "int", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periodization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Periodization_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Periodization_Professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "Professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Periodization_ClientId",
                table: "Periodization",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Periodization_ProfessionalId",
                table: "Periodization",
                column: "ProfessionalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_Professionals_ProfessionalId",
                table: "Exercise",
                column: "ProfessionalId",
                principalTable: "Professionals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MuscleGroup_Professionals_ProfessionalId",
                table: "MuscleGroup",
                column: "ProfessionalId",
                principalTable: "Professionals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PeriodizationTraining_Periodization_PeriodizationId",
                table: "PeriodizationTraining",
                column: "PeriodizationId",
                principalTable: "Periodization",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PeriodizationTraining_Training_TrainingId",
                table: "PeriodizationTraining",
                column: "TrainingId",
                principalTable: "Training",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutCategory_Professionals_ProfessionalId",
                table: "WorkoutCategory",
                column: "ProfessionalId",
                principalTable: "Professionals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_Professionals_ProfessionalId",
                table: "Exercise");

            migrationBuilder.DropForeignKey(
                name: "FK_MuscleGroup_Professionals_ProfessionalId",
                table: "MuscleGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_PeriodizationTraining_Periodization_PeriodizationId",
                table: "PeriodizationTraining");

            migrationBuilder.DropForeignKey(
                name: "FK_PeriodizationTraining_Training_TrainingId",
                table: "PeriodizationTraining");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutCategory_Professionals_ProfessionalId",
                table: "WorkoutCategory");

            migrationBuilder.DropTable(
                name: "Periodization");

            migrationBuilder.RenameColumn(
                name: "TrainingId",
                table: "PeriodizationTraining",
                newName: "ProfessionalId");

            migrationBuilder.RenameColumn(
                name: "PeriodizationId",
                table: "PeriodizationTraining",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_PeriodizationTraining_TrainingId",
                table: "PeriodizationTraining",
                newName: "IX_PeriodizationTraining_ProfessionalId");

            migrationBuilder.RenameIndex(
                name: "IX_PeriodizationTraining_PeriodizationId",
                table: "PeriodizationTraining",
                newName: "IX_PeriodizationTraining_ClientId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WorkoutCategory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "WorkoutCategory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "WorkoutCategory",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Training",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Training",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Training",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<Guid>(
                name: "PeriodizationTrainingId",
                table: "Training",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProfessionalTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ProfessionalTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "PeriodizationTraining",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateEnd",
                table: "PeriodizationTraining",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateStart",
                table: "PeriodizationTraining",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PeriodizationTraining",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PeriodizationTraining",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "WeeklyTrainingFrequency",
                table: "PeriodizationTraining",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MuscleGroup",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MuscleGroup",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "MuscleGroup",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Exercise",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Exercise",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Exercise",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_Training_PeriodizationTrainingId",
                table: "Training",
                column: "PeriodizationTrainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_Professionals_ProfessionalId",
                table: "Exercise",
                column: "ProfessionalId",
                principalTable: "Professionals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MuscleGroup_Professionals_ProfessionalId",
                table: "MuscleGroup",
                column: "ProfessionalId",
                principalTable: "Professionals",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PeriodizationTraining_Clients_ClientId",
                table: "PeriodizationTraining",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PeriodizationTraining_Professionals_ProfessionalId",
                table: "PeriodizationTraining",
                column: "ProfessionalId",
                principalTable: "Professionals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Training_PeriodizationTraining_PeriodizationTrainingId",
                table: "Training",
                column: "PeriodizationTrainingId",
                principalTable: "PeriodizationTraining",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutCategory_Professionals_ProfessionalId",
                table: "WorkoutCategory",
                column: "ProfessionalId",
                principalTable: "Professionals",
                principalColumn: "Id");
        }
    }
}
