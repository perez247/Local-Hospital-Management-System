using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChannelClinic.Migrations
{
    public partial class individual_company : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ForIndividual",
                table: "Companies",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForIndividual",
                table: "Companies");
        }
    }
}
