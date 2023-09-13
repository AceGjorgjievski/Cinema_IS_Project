using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cinema.Migrations
{
    public partial class AddedColumnCinemaUserInSeat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CinemaUserId",
                table: "Seats",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_CinemaUserId",
                table: "Seats",
                column: "CinemaUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_AspNetUsers_CinemaUserId",
                table: "Seats",
                column: "CinemaUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_AspNetUsers_CinemaUserId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_CinemaUserId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "CinemaUserId",
                table: "Seats");
        }
    }
}
