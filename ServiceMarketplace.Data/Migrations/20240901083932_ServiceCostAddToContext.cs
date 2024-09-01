using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceMarketplace.Data.Migrations
{
    /// <inheritdoc />
    public partial class ServiceCostAddToContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCost_Services_ServiceId",
                table: "ServiceCost");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceCost",
                table: "ServiceCost");

            migrationBuilder.RenameTable(
                name: "ServiceCost",
                newName: "ServiceCosts");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceCost_ServiceId",
                table: "ServiceCosts",
                newName: "IX_ServiceCosts_ServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceCosts",
                table: "ServiceCosts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCosts_Services_ServiceId",
                table: "ServiceCosts",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCosts_Services_ServiceId",
                table: "ServiceCosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceCosts",
                table: "ServiceCosts");

            migrationBuilder.RenameTable(
                name: "ServiceCosts",
                newName: "ServiceCost");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceCosts_ServiceId",
                table: "ServiceCost",
                newName: "IX_ServiceCost_ServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceCost",
                table: "ServiceCost",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCost_Services_ServiceId",
                table: "ServiceCost",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
