using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShareCar.Db.Migrations
{
    public partial class Rideanddrivernoteentitiesforeignkeysinverted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "RideId",
                table: "DriverNotes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DriverNotes_RideId",
                table: "DriverNotes",
                column: "RideId");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverNotes_Rides_RideId",
                table: "DriverNotes",
                column: "RideId",
                principalTable: "Rides",
                principalColumn: "RideId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverNotes_Rides_RideId",
                table: "DriverNotes");

            migrationBuilder.DropIndex(
                name: "IX_DriverNotes_RideId",
                table: "DriverNotes");

            migrationBuilder.DropColumn(
                name: "RideId",
                table: "DriverNotes");

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
    }
}
