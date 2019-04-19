using Microsoft.EntityFrameworkCore.Migrations;
using ShareCar.Db.Entities;
using System;
using System.Collections.Generic;

namespace ShareCar.Db.Migrations
{
    public partial class AddedGeometryToRoute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Geometry",
                table: "Routes",
                nullable: true               
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Geometry",
                table: "Routes");
        }
    }
}
