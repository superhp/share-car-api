using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShareCar.Db.Migrations
{
    public partial class noteentities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestId",
                table: "Requests",
                newName: "RideRequestId");

            migrationBuilder.AddPrimaryKey(
            name: "RideRequestId",
            table: "Requests",
            column: "RideRequestId");

            migrationBuilder.AddColumn<int>(
                name: "PassengerId",
                table: "Passengers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DriverNotes",
                columns: table => new
                {
                    DriverNoteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverNotes", x => x.DriverNoteId);
                });

            migrationBuilder.CreateTable(
                name: "RideRequestNotes",
                columns: table => new
                {
                    RideRequestNoteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RideRequestId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideRequestNotes", x => x.RideRequestNoteId);
                    table.ForeignKey(
                        name: "FK_RideRequestNotes_Requests_RideRequestId",
                        column: x => x.RideRequestId,
                        principalTable: "Requests",
                        principalColumn: "RideRequestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DriverSeenNotes",
                columns: table => new
                {
                    DriverSeenNoteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DriverNoteId = table.Column<int>(nullable: false),
                    Seen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverSeenNotes", x => x.DriverSeenNoteId);
                    table.ForeignKey(
                        name: "FK_DriverSeenNotes_DriverNotes_DriverNoteId",
                        column: x => x.DriverNoteId,
                        principalTable: "DriverNotes",
                        principalColumn: "DriverNoteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverSeenNotes_DriverNoteId",
                table: "DriverSeenNotes",
                column: "DriverNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_RideRequestNotes_RideRequestId",
                table: "RideRequestNotes",
                column: "RideRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverSeenNotes");

            migrationBuilder.DropTable(
                name: "RideRequestNotes");

            migrationBuilder.DropTable(
                name: "DriverNotes");

            migrationBuilder.DropColumn(
                name: "PassengerId",
                table: "Passengers");

            migrationBuilder.RenameColumn(
                name: "RideRequestId",
                table: "Requests",
                newName: "RequestId");
        }
    }
}
