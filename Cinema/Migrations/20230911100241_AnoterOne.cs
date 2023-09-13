using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cinema.Migrations
{
    public partial class AnoterOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DateTimeKeyId",
                table: "Seats",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_DateTimeKeyId",
                table: "Seats",
                column: "DateTimeKeyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_DateTimeKeys_DateTimeKeyId",
                table: "Seats",
                column: "DateTimeKeyId",
                principalTable: "DateTimeKeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_DateTimeKeys_DateTimeKeyId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_DateTimeKeyId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "DateTimeKeyId",
                table: "Seats");
        }
    }
}
