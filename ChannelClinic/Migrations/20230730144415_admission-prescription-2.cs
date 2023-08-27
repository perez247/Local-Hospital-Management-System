using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChannelClinic.Migrations
{
    public partial class admissionprescription2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdmissionPrescription_AppTickets_AppTicketId",
                table: "AdmissionPrescription");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketInventories_AdmissionPrescription_AdmissionPrescripti~",
                table: "TicketInventories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdmissionPrescription",
                table: "AdmissionPrescription");

            migrationBuilder.RenameTable(
                name: "AdmissionPrescription",
                newName: "AdmissionPrescriptions");

            migrationBuilder.RenameIndex(
                name: "IX_AdmissionPrescription_AppTicketId",
                table: "AdmissionPrescriptions",
                newName: "IX_AdmissionPrescriptions_AppTicketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdmissionPrescriptions",
                table: "AdmissionPrescriptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdmissionPrescriptions_AppTickets_AppTicketId",
                table: "AdmissionPrescriptions",
                column: "AppTicketId",
                principalTable: "AppTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketInventories_AdmissionPrescriptions_AdmissionPrescript~",
                table: "TicketInventories",
                column: "AdmissionPrescriptionId",
                principalTable: "AdmissionPrescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdmissionPrescriptions_AppTickets_AppTicketId",
                table: "AdmissionPrescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketInventories_AdmissionPrescriptions_AdmissionPrescript~",
                table: "TicketInventories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdmissionPrescriptions",
                table: "AdmissionPrescriptions");

            migrationBuilder.RenameTable(
                name: "AdmissionPrescriptions",
                newName: "AdmissionPrescription");

            migrationBuilder.RenameIndex(
                name: "IX_AdmissionPrescriptions_AppTicketId",
                table: "AdmissionPrescription",
                newName: "IX_AdmissionPrescription_AppTicketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdmissionPrescription",
                table: "AdmissionPrescription",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdmissionPrescription_AppTickets_AppTicketId",
                table: "AdmissionPrescription",
                column: "AppTicketId",
                principalTable: "AppTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketInventories_AdmissionPrescription_AdmissionPrescripti~",
                table: "TicketInventories",
                column: "AdmissionPrescriptionId",
                principalTable: "AdmissionPrescription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
