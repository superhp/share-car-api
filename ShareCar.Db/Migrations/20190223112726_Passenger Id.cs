using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShareCar.Db.Migrations
{
    // Running this migration will cause an error. Sql code droping passengerId was generated for some of unkown reason,
    // because nor Passenger table, nor entity doesn't have Alternate key PassengerId
    public partial class UnauthorizesUserentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
   /*        migrationBuilder.DropUniqueConstraint(
                name: "AK_Passengers_PassengerId",
               table: "Passengers");

            migrationBuilder.DropColumn(
                name: "PassengerId",
                table: "Passengers");*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.AddColumn<int>(
                name: "PassengerId",
                table: "Passengers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Passengers_PassengerId",
                table: "Passengers",
                column: "PassengerId");*/
        }
    }
}
