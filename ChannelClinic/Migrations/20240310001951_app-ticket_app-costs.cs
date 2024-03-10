using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChannelClinic.Migrations
{
    public partial class appticket_appcosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AppTicketPartId",
                table: "AppCosts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppCosts_AppTicketPartId",
                table: "AppCosts",
                column: "AppTicketPartId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCosts_AppTickets_AppTicketPartId",
                table: "AppCosts",
                column: "AppTicketPartId",
                principalTable: "AppTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCosts_AppTickets_AppTicketPartId",
                table: "AppCosts");

            migrationBuilder.DropIndex(
                name: "IX_AppCosts_AppTicketPartId",
                table: "AppCosts");

            migrationBuilder.DropColumn(
                name: "AppTicketPartId",
                table: "AppCosts");
        }
    }
}
