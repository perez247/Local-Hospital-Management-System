using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChannelClinic.Migrations
{
    public partial class company_apptickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "AppTickets",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppTickets_CompanyId",
                table: "AppTickets",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppTickets_Companies_CompanyId",
                table: "AppTickets",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppTickets_Companies_CompanyId",
                table: "AppTickets");

            migrationBuilder.DropIndex(
                name: "IX_AppTickets_CompanyId",
                table: "AppTickets");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "AppTickets");
        }
    }
}
