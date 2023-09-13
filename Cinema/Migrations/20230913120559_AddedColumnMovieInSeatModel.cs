using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cinema.Migrations
{
    public partial class AddedColumnMovieInSeatModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MovieId",
                table: "Seats",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_MovieId",
                table: "Seats",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Movies_MovieId",
                table: "Seats",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Movies_MovieId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_MovieId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "Seats");
        }
    }
}
