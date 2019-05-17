using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShareCar.Db.Migrations
{
    public partial class Passengertablerecreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Passengers");

            migrationBuilder.CreateTable(
          name: "Passengers",
          columns: table => new
          {
              PassengerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
              Email = table.Column<string>(maxLength: 256, nullable: false),
              RideId = table.Column<int>(nullable: false),
              Completed = table.Column<bool>(nullable: false),
              PassengerResponded = table.Column<bool>(nullable: false)
          },
          constraints: table =>
          {
              table.ForeignKey(
                  name: "FK_Passengers_Rides_RideId",
                  column: x => x.RideId,
                  principalTable: "Rides",
                  principalColumn: "RideId",
                  onDelete: ReferentialAction.Cascade);
              table.ForeignKey(
                  name: "FK_Passengers_AspNetUsers_Email",
                  column: x => x.Email,
                  principalTable: "AspNetUsers",
                  principalColumn: "Email",
                  onDelete: ReferentialAction.Restrict);
          });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
