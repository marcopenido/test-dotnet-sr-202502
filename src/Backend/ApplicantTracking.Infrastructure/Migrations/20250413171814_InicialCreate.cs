using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApplicantTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InicialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LastUpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Timelines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTimelineType = table.Column<byte>(type: "tinyint", nullable: false),
                    IdAggregateRoot = table.Column<int>(type: "int", nullable: false),
                    OldData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timelines", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Candidates",
                columns: new[] { "Id", "Birthdate", "CreatedAt", "Email", "LastUpdateAt", "Name", "Surname" },
                values: new object[,]
                {
                    { 1, new DateTime(1990, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 13, 17, 18, 14, 885, DateTimeKind.Utc).AddTicks(5286), "john@email.com", null, "John", "Doe" },
                    { 2, new DateTime(1990, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 13, 17, 18, 14, 885, DateTimeKind.Utc).AddTicks(5309), "paul@email.com", null, "Paul", "Doe" },
                    { 3, new DateTime(1990, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 13, 17, 18, 14, 885, DateTimeKind.Utc).AddTicks(5314), "Erick@email.com", null, "Erick", "Doe" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "Timelines");
        }
    }
}
