using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShareCar.Db.Migrations
{
    public partial class RideRequestentiiesdependecyivered : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Rides_RideId",
                table: "Requests");

            migrationBuilder.AlterColumn<int>(
                name: "RideId",
                table: "Requests",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Rides_RideId",
                table: "Requests",
                column: "RideId",
                principalTable: "Rides",
                principalColumn: "RideId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Rides_RideId",
                table: "Requests");

            migrationBuilder.AlterColumn<int>(
                name: "RideId",
                table: "Requests",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Rides_RideId",
                table: "Requests",
                column: "RideId",
                principalTable: "Rides",
                principalColumn: "RideId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
