using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Training.Data.Migrations
{
    /// <inheritdoc />
    public partial class Addedstartingtablesrelatedtoexercisesandtraining : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MuscleGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfessionalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuscleGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MuscleGroup_Professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "Professionals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PeriodizationTraining",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfessionalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WeeklyTrainingFrequency = table.Column<int>(type: "int", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodizationTraining", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PeriodizationTraining_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeriodizationTraining_Professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "Professionals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfessionalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutCategory_Professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "Professionals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Training",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PeriodizationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PeriodizationTrainingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ProfessionalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Training", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Training_PeriodizationTraining_PeriodizationTrainingId",
                        column: x => x.PeriodizationTrainingId,
                        principalTable: "PeriodizationTraining",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Training_Professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "Professionals",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Exercise",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfessionalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkoutCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MuscleGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlMedia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exercise_MuscleGroup_MuscleGroupId",
                        column: x => x.MuscleGroupId,
                        principalTable: "MuscleGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exercise_Professionals_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalTable: "Professionals",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Exercise_WorkoutCategory_WorkoutCategoryId",
                        column: x => x.WorkoutCategoryId,
                        principalTable: "WorkoutCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Professionals",
                keyColumn: "Id",
                keyValue: new Guid("f0e1d2c3-5b6a-7d8e-9f0c-1a2b3e4d5c6f"),
                column: "Password",
                value: "$2a$11$e34Jq7J0U4e8FT6DjnQilOXYWvKtx7iIz5jSid5YMwHsxO.pbNgTS");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_MuscleGroupId",
                table: "Exercise",
                column: "MuscleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_ProfessionalId",
                table: "Exercise",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_WorkoutCategoryId",
                table: "Exercise",
                column: "WorkoutCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MuscleGroup_ProfessionalId",
                table: "MuscleGroup",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_PeriodizationTraining_ClientId",
                table: "PeriodizationTraining",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_PeriodizationTraining_ProfessionalId",
                table: "PeriodizationTraining",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_Training_PeriodizationTrainingId",
                table: "Training",
                column: "PeriodizationTrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_Training_ProfessionalId",
                table: "Training",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutCategory_ProfessionalId",
                table: "WorkoutCategory",
                column: "ProfessionalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exercise");

            migrationBuilder.DropTable(
                name: "Training");

            migrationBuilder.DropTable(
                name: "MuscleGroup");

            migrationBuilder.DropTable(
                name: "WorkoutCategory");

            migrationBuilder.DropTable(
                name: "PeriodizationTraining");

            migrationBuilder.UpdateData(
                table: "Professionals",
                keyColumn: "Id",
                keyValue: new Guid("f0e1d2c3-5b6a-7d8e-9f0c-1a2b3e4d5c6f"),
                column: "Password",
                value: "$2a$12$X7E9OQ9y9.H7rYVcOGBZQ.uoP.eNgpVpDw2G9hI9T7upICGq8kgy6");
        }
    }
}
