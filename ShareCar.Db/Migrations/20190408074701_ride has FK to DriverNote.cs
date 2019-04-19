using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShareCar.Db.Migrations
{
    public partial class ridehasFKtoDriverNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DriverNoteId",
                table: "Rides",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rides_DriverNoteId",
                table: "Rides",
                column: "DriverNoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_DriverNotes_DriverNoteId",
                table: "Rides",
                column: "DriverNoteId",
                principalTable: "DriverNotes",
                principalColumn: "DriverNoteId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_DriverNotes_DriverNoteId",
                table: "Rides");

            migrationBuilder.DropIndex(
                name: "IX_Rides_DriverNoteId",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "DriverNoteId",
                table: "Rides");
        }
    }
}
