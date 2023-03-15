using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChannelClinic.Migrations
{
    public partial class dockors_description : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrescribedAdmission",
                table: "TicketInventories");

            migrationBuilder.RenameColumn(
                name: "PrescribedSurgeryDescription",
                table: "TicketInventories",
                newName: "FinanceDescription");

            migrationBuilder.RenameColumn(
                name: "PrescribedPharmacyDosage",
                table: "TicketInventories",
                newName: "DoctorsPrescription");

            migrationBuilder.RenameColumn(
                name: "PrescribedLabRadiologyFeature",
                table: "TicketInventories",
                newName: "DepartmentDescription");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FinanceDescription",
                table: "TicketInventories",
                newName: "PrescribedSurgeryDescription");

            migrationBuilder.RenameColumn(
                name: "DoctorsPrescription",
                table: "TicketInventories",
                newName: "PrescribedPharmacyDosage");

            migrationBuilder.RenameColumn(
                name: "DepartmentDescription",
                table: "TicketInventories",
                newName: "PrescribedLabRadiologyFeature");

            migrationBuilder.AddColumn<string>(
                name: "PrescribedAdmission",
                table: "TicketInventories",
                type: "character varying(5000)",
                maxLength: 5000,
                nullable: true);
        }
    }
}
