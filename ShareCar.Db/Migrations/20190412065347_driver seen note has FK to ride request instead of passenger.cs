using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShareCar.Db.Migrations
{
    public partial class driverseennotehasFKtoriderequestinsteadofpassenger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverSeenNotes_Passengers_PassengerId",
                table: "DriverSeenNotes");

            migrationBuilder.DropIndex(
                name: "IX_DriverSeenNotes_PassengerEmail_PassengerRideId",
                table: "DriverSeenNotes");

            migrationBuilder.DropColumn(
                name: "PassengerEmail",
                table: "DriverSeenNotes");

            migrationBuilder.DropColumn(
                name: "PassengerRideId",
                table: "DriverSeenNotes");

            migrationBuilder.RenameColumn(
                name: "PassengerId",
                table: "DriverSeenNotes",
                newName: "RideRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverSeenNotes_RideRequestId",
                table: "DriverSeenNotes",
                column: "RideRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverSeenNotes_Requests_RideRequestId",
                table: "DriverSeenNotes",
                column: "RideRequestId",
                principalTable: "Requests",
                principalColumn: "RideRequestId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverSeenNotes_Requests_RideRequestId",
                table: "DriverSeenNotes");

            migrationBuilder.DropIndex(
                name: "IX_DriverSeenNotes_RideRequestId",
                table: "DriverSeenNotes");

            migrationBuilder.RenameColumn(
                name: "RideRequestId",
                table: "DriverSeenNotes",
                newName: "PassengerId");

            migrationBuilder.AddColumn<string>(
                name: "PassengerEmail",
                table: "DriverSeenNotes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PassengerRideId",
                table: "DriverSeenNotes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DriverSeenNotes_PassengerEmail_PassengerRideId",
                table: "DriverSeenNotes",
                columns: new[] { "PassengerEmail", "PassengerRideId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DriverSeenNotes_Passengers_PassengerEmail_PassengerRideId",
                table: "DriverSeenNotes",
                columns: new[] { "PassengerEmail", "PassengerRideId" },
                principalTable: "Passengers",
                principalColumns: new[] { "Email", "RideId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
