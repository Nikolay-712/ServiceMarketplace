using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceMarketplace.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOfferedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OfferedAtId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OfferedAt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameBg = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferedAt", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_OfferedAtId",
                table: "Services",
                column: "OfferedAtId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_OfferedAt_OfferedAtId",
                table: "Services",
                column: "OfferedAtId",
                principalTable: "OfferedAt",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_OfferedAt_OfferedAtId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "OfferedAt");

            migrationBuilder.DropIndex(
                name: "IX_Services_OfferedAtId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "OfferedAtId",
                table: "Services");
        }
    }
}
