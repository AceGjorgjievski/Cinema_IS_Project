using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Cinema.Migrations
{
    public partial class ModifiedColumnSeatsInOrderEntityToListOfIntegersForStoringSeatNumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Orders_OrderId",
                table: "Seats");

            migrationBuilder.DropIndex(
                name: "IX_Seats_OrderId",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Seats");

            migrationBuilder.AddColumn<List<int>>(
                name: "Seats",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seats",
                table: "Orders");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Seats",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_OrderId",
                table: "Seats",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Orders_OrderId",
                table: "Seats",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
