using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChannelClinic.Migrations
{
    public partial class admissionprescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AdmissionPrescriptionId",
                table: "TicketInventories",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AdmissionPrescription",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppTicketId = table.Column<Guid>(type: "uuid", nullable: true),
                    OverallDescription = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: true),
                    AppTicketStatus = table.Column<int>(type: "integer", nullable: false),
                    AppInventoryType = table.Column<int>(type: "integer", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdmissionPrescription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdmissionPrescription_AppTickets_AppTicketId",
                        column: x => x.AppTicketId,
                        principalTable: "AppTickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketInventories_AdmissionPrescriptionId",
                table: "TicketInventories",
                column: "AdmissionPrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionPrescription_AppTicketId",
                table: "AdmissionPrescription",
                column: "AppTicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketInventories_AdmissionPrescription_AdmissionPrescripti~",
                table: "TicketInventories",
                column: "AdmissionPrescriptionId",
                principalTable: "AdmissionPrescription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketInventories_AdmissionPrescription_AdmissionPrescripti~",
                table: "TicketInventories");

            migrationBuilder.DropTable(
                name: "AdmissionPrescription");

            migrationBuilder.DropIndex(
                name: "IX_TicketInventories_AdmissionPrescriptionId",
                table: "TicketInventories");

            migrationBuilder.DropColumn(
                name: "AdmissionPrescriptionId",
                table: "TicketInventories");
        }
    }
}
