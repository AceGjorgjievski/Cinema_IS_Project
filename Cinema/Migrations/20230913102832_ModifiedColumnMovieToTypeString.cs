using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cinema.Migrations
{
    public partial class ModifiedColumnMovieToTypeString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Movies_MovieId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_MovieId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "MovieName",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovieName",
                table: "Orders");

            migrationBuilder.AddColumn<Guid>(
                name: "MovieId",
                table: "Orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_MovieId",
                table: "Orders",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Movies_MovieId",
                table: "Orders",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
