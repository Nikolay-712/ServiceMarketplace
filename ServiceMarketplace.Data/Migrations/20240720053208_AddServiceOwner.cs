using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceMarketplace.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Services",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Services_OwnerId",
                table: "Services",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_AspNetUsers_OwnerId",
                table: "Services",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_AspNetUsers_OwnerId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_OwnerId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Services");
        }
    }
}
