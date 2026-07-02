using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SwimSuite.Data.Migrations
{
    [Migration("20260702120000_AddTrainingSchedule")]
    public partial class AddTrainingSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ClubId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 120, nullable: false),
                    Description = table.Column<string>(maxLength: 400, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingGroups_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingBlocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ClubId = table.Column<Guid>(nullable: false),
                    TrainingGroupId = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateOnly>(nullable: false),
                    StartTime = table.Column<TimeOnly>(nullable: false),
                    EndTime = table.Column<TimeOnly>(nullable: false),
                    Location = table.Column<string>(maxLength: 160, nullable: true),
                    Notes = table.Column<string>(maxLength: 400, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingBlocks_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingBlocks_TrainingGroups_TrainingGroupId",
                        column: x => x.TrainingGroupId,
                        principalTable: "TrainingGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingBlocks_ClubId",
                table: "TrainingBlocks",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingBlocks_TrainingGroupId",
                table: "TrainingBlocks",
                column: "TrainingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingGroups_ClubId",
                table: "TrainingGroups",
                column: "ClubId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "TrainingBlocks");
            migrationBuilder.DropTable(name: "TrainingGroups");
        }
    }
}
