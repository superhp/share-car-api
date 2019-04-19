using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShareCar.Db.Migrations
{
    public partial class SeenByDriver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SeenByDriver",
                table: "Requests",
                nullable: true);
            migrationBuilder.AddColumn<bool>(
                name: "SeenByPassenger",
                table: "Requests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "SeenByDriver",
               table: "Requests");
            migrationBuilder.DropColumn(
               name: "SeenByPassenger",
               table: "Requests");
        }
    }
}
