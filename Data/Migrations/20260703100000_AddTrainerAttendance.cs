using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SwimSuite.Data.Migrations
{
    [Migration("20260703100000_AddTrainerAttendance")]
    public partial class AddTrainerAttendance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainerAttendances",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ClubId = table.Column<Guid>(nullable: false),
                    TrainingBlockId = table.Column<Guid>(nullable: false),
                    TrainerId = table.Column<Guid>(nullable: false),
                    IsPresent = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(maxLength: 400, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainerAttendances_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainerAttendances_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainerAttendances_TrainingBlocks_TrainingBlockId",
                        column: x => x.TrainingBlockId,
                        principalTable: "TrainingBlocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainerAttendances_ClubId",
                table: "TrainerAttendances",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerAttendances_TrainerId",
                table: "TrainerAttendances",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerAttendances_TrainingBlockId_TrainerId",
                table: "TrainerAttendances",
                columns: new[] { "TrainingBlockId", "TrainerId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "TrainerAttendances");
        }
    }
}
