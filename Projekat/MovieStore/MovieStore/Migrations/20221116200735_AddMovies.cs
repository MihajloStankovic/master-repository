using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieStore.Migrations
{
    public partial class AddMovies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Director = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    MovieName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    releaseDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
