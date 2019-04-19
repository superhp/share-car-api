using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShareCar.Db.Migrations
{
    public partial class newproprtiesinUserentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "VerificationCode",
                table: "UnauthorizedUsers",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<string>(
                name: "CognizantEmail",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookEmail",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FacebookVerified",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "GoogleEmail",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GoogleId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GoogleVerified",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CognizantEmail",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FacebookEmail",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FacebookVerified",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GoogleEmail",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GoogleId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GoogleVerified",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<long>(
                name: "VerificationCode",
                table: "UnauthorizedUsers",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
