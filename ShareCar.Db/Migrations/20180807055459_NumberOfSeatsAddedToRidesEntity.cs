using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShareCar.Db.Migrations
{
    public partial class NumberOfSeatsAddedToRidesEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Geometry",
                table: "Routes",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfSeats",
                table: "Rides",
                nullable: false,
                defaultValue: 4);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_Geometry",
                table: "Routes",
                column: "Geometry",
                unique: true,
                filter: "[Geometry] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Routes_Geometry",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "NumberOfSeats",
                table: "Rides");

            migrationBuilder.AlterColumn<string>(
                name: "Geometry",
                table: "Routes",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
