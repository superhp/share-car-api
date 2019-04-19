using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShareCar.Db.Migrations
{
    public partial class AddedForeignKeysToRouteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Route_RouteId",
                table: "Rides"
                );

            migrationBuilder.DropPrimaryKey(
                name: "PK_Route",
                table: "Route");

            migrationBuilder.RenameTable(
                name: "Route",
                newName: "Routes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Routes",
                table: "Routes",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_FromId",
                table: "Routes",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ToId",
                table: "Routes",
                column: "ToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Routes_RouteId",
                table: "Rides",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "RouteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Addresses_FromId",
                table: "Routes",
                column: "FromId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Addresses_ToId",
                table: "Routes",
                column: "ToId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Routes_RouteId",
                table: "Rides");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Addresses_FromId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Addresses_ToId",
                table: "Routes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Routes",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_FromId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_ToId",
                table: "Routes");

            migrationBuilder.RenameTable(
                name: "Routes",
                newName: "Route");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Route",
                table: "Route",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Route_RouteId",
                table: "Rides",
                column: "RouteId",
                principalTable: "Route",
                principalColumn: "RouteId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
