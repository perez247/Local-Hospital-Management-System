using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChannelClinic.Migrations
{
    public partial class prescriptdoctor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DoctorId",
                table: "AdmissionPrescriptions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionPrescriptions_DoctorId",
                table: "AdmissionPrescriptions",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdmissionPrescriptions_AspNetUsers_DoctorId",
                table: "AdmissionPrescriptions",
                column: "DoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdmissionPrescriptions_AspNetUsers_DoctorId",
                table: "AdmissionPrescriptions");

            migrationBuilder.DropIndex(
                name: "IX_AdmissionPrescriptions_DoctorId",
                table: "AdmissionPrescriptions");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "AdmissionPrescriptions");
        }
    }
}
