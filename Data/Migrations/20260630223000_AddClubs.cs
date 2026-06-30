using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SwimSuite.Data.Migrations
{
    public partial class AddClubs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 160, nullable: false),
                    RegistrationNumber = table.Column<string>(maxLength: 40, nullable: true),
                    Email = table.Column<string>(maxLength: 160, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 80, nullable: true),
                    Address = table.Column<string>(maxLength: 240, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clubs");
        }
    }
}
