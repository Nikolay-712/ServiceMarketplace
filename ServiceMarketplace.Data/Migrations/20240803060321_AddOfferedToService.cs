using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceMarketplace.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOfferedToService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_OfferedAt_OfferedAtId",
                table: "Services");

            migrationBuilder.AlterColumn<int>(
                name: "OfferedAtId",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_OfferedAt_OfferedAtId",
                table: "Services",
                column: "OfferedAtId",
                principalTable: "OfferedAt",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_OfferedAt_OfferedAtId",
                table: "Services");

            migrationBuilder.AlterColumn<int>(
                name: "OfferedAtId",
                table: "Services",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_OfferedAt_OfferedAtId",
                table: "Services",
                column: "OfferedAtId",
                principalTable: "OfferedAt",
                principalColumn: "Id");
        }
    }
}
