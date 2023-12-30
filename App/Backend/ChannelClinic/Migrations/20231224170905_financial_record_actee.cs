using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChannelClinic.Migrations
{
    public partial class financial_record_actee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ActorId",
                table: "FinancialRecords",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ManualEntry",
                table: "FinancialRecords",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialRecords_ActorId",
                table: "FinancialRecords",
                column: "ActorId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialRecords_AspNetUsers_ActorId",
                table: "FinancialRecords",
                column: "ActorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialRecords_AspNetUsers_ActorId",
                table: "FinancialRecords");

            migrationBuilder.DropIndex(
                name: "IX_FinancialRecords_ActorId",
                table: "FinancialRecords");

            migrationBuilder.DropColumn(
                name: "ActorId",
                table: "FinancialRecords");

            migrationBuilder.DropColumn(
                name: "ManualEntry",
                table: "FinancialRecords");
        }
    }
}
