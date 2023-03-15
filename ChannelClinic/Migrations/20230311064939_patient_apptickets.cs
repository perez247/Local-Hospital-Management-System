using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChannelClinic.Migrations
{
    public partial class patient_apptickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PatientId",
                table: "AppTickets",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppTickets_PatientId",
                table: "AppTickets",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppTickets_Patients_PatientId",
                table: "AppTickets",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppTickets_Patients_PatientId",
                table: "AppTickets");

            migrationBuilder.DropIndex(
                name: "IX_AppTickets_PatientId",
                table: "AppTickets");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "AppTickets");
        }
    }
}
