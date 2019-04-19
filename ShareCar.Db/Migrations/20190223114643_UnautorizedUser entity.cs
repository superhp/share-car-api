using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShareCar.Db.Migrations
{
    public partial class UnautorizedUserentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnauthorizedUsers",
                columns: table => new
                {
                    UnauthorizedUserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    VerificationCode = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnauthorizedUsers", x => x.UnauthorizedUserId);
                    table.ForeignKey(
            name: "FK_UnauthorizedUsers_AspNetUsers_Email",
            column: x => x.Email,
            principalTable: "AspNetUsers",
            principalColumn: "Email",
            onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnauthorizedUsers_Email",
                table: "UnauthorizedUsers",
                column: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnauthorizedUsers");
        }
    }
}
