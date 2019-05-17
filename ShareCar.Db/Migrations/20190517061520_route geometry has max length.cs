using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShareCar.Db.Migrations
{
    public partial class routegeometryhasmaxlength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex("IX_Routes_Geometry", "Routes");

            migrationBuilder.AlterColumn<string>(
                name: "Geometry",
                table: "Routes",
                type: "NVARCHAR(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfSeats",
                table: "Rides",
                nullable: false,
                defaultValue: 4,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfSeats",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 4);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Geometry",
                table: "Routes",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(MAX)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfSeats",
                table: "Rides",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 4);

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfSeats",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 4,
                oldClrType: typeof(int));
        }
    }
}
