using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cinema.Migrations
{
    public partial class AddedOneToManyRelationshipWithCinemaUserAndOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Orders_OrderId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_OrderId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "CinemaUserId",
                table: "Orders",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CinemaUserId",
                table: "Orders",
                column: "CinemaUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_CinemaUserId",
                table: "Orders",
                column: "CinemaUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_CinemaUserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CinemaUserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CinemaUserId",
                table: "Orders");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OrderId",
                table: "AspNetUsers",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Orders_OrderId",
                table: "AspNetUsers",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
