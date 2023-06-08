using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChannelClinic.Migrations
{
    public partial class inventory_dependant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppInventoryDependencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppInventoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    DependantId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppInventoryDependencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppInventoryDependencies_AppInventories_AppInventoryId",
                        column: x => x.AppInventoryId,
                        principalTable: "AppInventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppInventoryDependencies_AppInventories_DependantId",
                        column: x => x.DependantId,
                        principalTable: "AppInventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppInventoryDependencies_AppInventoryId",
                table: "AppInventoryDependencies",
                column: "AppInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppInventoryDependencies_DependantId",
                table: "AppInventoryDependencies",
                column: "DependantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppInventoryDependencies");
        }
    }
}
