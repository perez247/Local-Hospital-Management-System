using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChannelClinic.Migrations
{
    public partial class financial_app_cost_total : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalAppCosts",
                table: "FinancialRecords",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAppCosts",
                table: "FinancialRecords");
        }
    }
}
