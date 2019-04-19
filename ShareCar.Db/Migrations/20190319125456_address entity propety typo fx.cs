using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShareCar.Db.Migrations
{
    public partial class addressentitypropetytypofx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Addresses_AddressId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_DriverEmail",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_PassengerEmail",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Rides_RideId",
                table: "Requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Requests",
                table: "Requests");

       //     migrationBuilder.RenameTable(
        //        name: "RideRequests",
         //       newName: "Requests");
         /*
            migrationBuilder.RenameIndex(
                name: "IX_Requests_RideId",
                table: "Requests",
                newName: "IX_Requests_RideId");

            migrationBuilder.RenameIndex(
                name: "IX_RideRequests_PassengerEmail",
                table: "Requests",
                newName: "IX_Requests_PassengerEmail");

            migrationBuilder.RenameIndex(
                name: "IX_RideRequests_DriverEmail",
                table: "Requests",
                newName: "IX_Requests_DriverEmail");

            migrationBuilder.RenameIndex(
                name: "IX_RideRequests_AddressId",
                table: "Requests",
                newName: "IX_Requests_AddressId");
            */
            migrationBuilder.RenameColumn(
                name: "Longtitude",
                table: "Addresses",
                newName: "Longitude");
            /*
            migrationBuilder.AddPrimaryKey(
                name: "PK_Requests",
                table: "Requests",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Addresses_AddressId",
                table: "Requests",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_DriverEmail",
                table: "Requests",
                column: "DriverEmail",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_PassengerEmail",
                table: "Requests",
                column: "PassengerEmail",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Rides_RideId",
                table: "Requests",
                column: "RideId",
                principalTable: "Rides",
                principalColumn: "RideId",
                onDelete: ReferentialAction.Cascade);*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {/*
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Addresses_AddressId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_DriverEmail",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_PassengerEmail",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Rides_RideId",
                table: "Requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Requests",
                table: "Requests");
            */
   //         migrationBuilder.RenameTable(
     //           name: "Requests",
      //          newName: "RideRequests");

       //     migrationBuilder.RenameIndex(
     //           name: "IX_Requests_RideId",
      //          table: "RideRequests",
       //         newName: "IX_RideRequests_RideId");

        //    migrationBuilder.RenameIndex(
        //        name: "IX_Requests_PassengerEmail",
         //       table: "RideRequests",
         //       newName: "IX_RideRequests_PassengerEmail");
         /*
            migrationBuilder.RenameIndex(
                name: "IX_Requests_DriverEmail",
                table: "RideRequests",
                newName: "IX_RideRequests_DriverEmail");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_AddressId",
                table: "RideRequests",
                newName: "IX_RideRequests_AddressId");
            */
            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Addresses",
                newName: "Longtitude");
            /*
            migrationBuilder.AddPrimaryKey(
                name: "PK_Requests",
                table: "Requests",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Addresses_AddressId",
                table: "Requests",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_DriverEmail",
                table: "Requests",
                column: "DriverEmail",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_PassengerEmail",
                table: "Requests",
                column: "PassengerEmail",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Rides_RideId",
                table: "Requests",
                column: "RideId",
                principalTable: "Rides",
                principalColumn: "RideId",
                onDelete: ReferentialAction.Cascade);*/
        }
    }
}
