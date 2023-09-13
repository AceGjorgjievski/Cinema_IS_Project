using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cinema.Migrations
{
    public partial class RemovedSeatsFromMovieBookDtoAndRemovedMovieAndCinemaUserInSeatAndAddedAvailableTimesInMovieBookDto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_AspNetUsers_CinemaUserId",
                table: "Seats");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Movies_MovieId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_CinemaUserId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_MovieId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "CinemaUserId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "Seats");

            migrationBuilder.AddColumn<Guid>(
                name: "CinemaUserId",
                table: "SeatMaps",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MovieId",
                table: "SeatMaps",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeatMaps_CinemaUserId",
                table: "SeatMaps",
                column: "CinemaUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatMaps_MovieId",
                table: "SeatMaps",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_SeatMaps_AspNetUsers_CinemaUserId",
                table: "SeatMaps",
                column: "CinemaUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SeatMaps_Movies_MovieId",
                table: "SeatMaps",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeatMaps_AspNetUsers_CinemaUserId",
                table: "SeatMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_SeatMaps_Movies_MovieId",
                table: "SeatMaps");

            migrationBuilder.DropIndex(
                name: "IX_SeatMaps_CinemaUserId",
                table: "SeatMaps");

            migrationBuilder.DropIndex(
                name: "IX_SeatMaps_MovieId",
                table: "SeatMaps");

            migrationBuilder.DropColumn(
                name: "CinemaUserId",
                table: "SeatMaps");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "SeatMaps");

            migrationBuilder.AddColumn<Guid>(
                name: "CinemaUserId",
                table: "Seats",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MovieId",
                table: "Seats",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_CinemaUserId",
                table: "Seats",
                column: "CinemaUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_MovieId",
                table: "Seats",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_AspNetUsers_CinemaUserId",
                table: "Seats",
                column: "CinemaUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Movies_MovieId",
                table: "Seats",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
