using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShareCar.Db.Migrations
{
    public partial class DriverSeenNotehasFKtoPassenger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PassengerEmail",
                table: "DriverSeenNotes",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
            name: "PassengerId",
            table: "Passengers",
            column: "PassengerId");

            migrationBuilder.AddColumn<int>(
                name: "PassengerId",
                table: "DriverSeenNotes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PassengerRideId",
                table: "DriverSeenNotes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DriverSeenNotes_PassengerEmail_PassengerRideId",
                table: "DriverSeenNotes",
                columns: new[] { "PassengerEmail", "PassengerRideId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DriverSeenNotes_Passengers_PassengerId",
                table: "DriverSeenNotes",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "PassengerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverSeenNotes_Passengers_PassengerEmail_PassengerRideId",
                table: "DriverSeenNotes");

            migrationBuilder.DropIndex(
                name: "IX_DriverSeenNotes_PassengerEmail_PassengerRideId",
                table: "DriverSeenNotes");

            migrationBuilder.DropColumn(
                name: "PassengerEmail",
                table: "DriverSeenNotes");

            migrationBuilder.DropColumn(
                name: "PassengerId",
                table: "DriverSeenNotes");

            migrationBuilder.DropColumn(
                name: "PassengerRideId",
                table: "DriverSeenNotes");
        }
    }
}
