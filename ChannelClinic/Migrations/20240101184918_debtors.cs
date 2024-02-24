using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChannelClinic.Migrations
{
    public partial class debtors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ConcludedPrice",
                table: "TicketInventories",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AppTicketId",
                table: "AppCosts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TicketInventoryDebtor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PayerId = table.Column<Guid>(type: "uuid", nullable: true),
                    TicketInventoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketInventoryDebtor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketInventoryDebtor_AspNetUsers_PayerId",
                        column: x => x.PayerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketInventoryDebtor_TicketInventories_TicketInventoryId",
                        column: x => x.TicketInventoryId,
                        principalTable: "TicketInventories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketInventoryDebtor_PayerId",
                table: "TicketInventoryDebtor",
                column: "PayerId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketInventoryDebtor_TicketInventoryId",
                table: "TicketInventoryDebtor",
                column: "TicketInventoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketInventoryDebtor");

            migrationBuilder.DropColumn(
                name: "ConcludedPrice",
                table: "TicketInventories");

            migrationBuilder.DropColumn(
                name: "AppTicketId",
                table: "AppCosts");
        }
    }
}
