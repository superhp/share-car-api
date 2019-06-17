using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShareCar.Db.Migrations
{
    public partial class HomeAddresspropertyaddedtouserentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HomeAddressId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_HomeAddressId",
                table: "AspNetUsers",
                column: "HomeAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Addresses_HomeAddressId",
                table: "AspNetUsers",
                column: "HomeAddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Addresses_HomeAddressId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_HomeAddressId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HomeAddressId",
                table: "AspNetUsers");
        }
    }
}
