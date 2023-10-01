using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChannelClinic.Migrations
{
    public partial class ticketinventory_timetaken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeGiven",
                table: "TicketInventories",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeGiven",
                table: "TicketInventories");
        }
    }
}
