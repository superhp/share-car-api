using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShareCar.Db.Migrations
{
    public partial class AddedRouteTableAndLinkedWithRidesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Addresses_FromId",
                table: "Rides");

            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Addresses_ToId",
                table: "Rides");

            migrationBuilder.DropIndex(
                name: "IX_Rides_FromId",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "FromId",
                table: "Rides");

            migrationBuilder.RenameColumn(
                name: "ToId",
                table: "Rides",
                newName: "RouteId");

            migrationBuilder.RenameIndex(
                name: "IX_Rides_ToId",
                table: "Rides",
                newName: "IX_Rides_RouteId");

            migrationBuilder.CreateTable(
                name: "Route",
                columns: table => new
                {
                    RouteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FromId = table.Column<int>(nullable: false),
                    ToId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Route", x => x.RouteId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Route_RouteId",
                table: "Rides",
                column: "RouteId",
                principalTable: "Route",
                principalColumn: "RouteId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Route_RouteId",
                table: "Rides");

            migrationBuilder.DropTable(
                name: "Route");

            migrationBuilder.RenameColumn(
                name: "RouteId",
                table: "Rides",
                newName: "ToId");

            migrationBuilder.RenameIndex(
                name: "IX_Rides_RouteId",
                table: "Rides",
                newName: "IX_Rides_ToId");

            migrationBuilder.AddColumn<int>(
                name: "FromId",
                table: "Rides",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rides_FromId",
                table: "Rides",
                column: "FromId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Addresses_FromId",
                table: "Rides",
                column: "FromId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Addresses_ToId",
                table: "Rides",
                column: "ToId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
