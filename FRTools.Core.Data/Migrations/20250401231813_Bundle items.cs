using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FRTools.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class Bundleitems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FRItem_Id",
                table: "FRItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FRItems_FRItem_Id",
                table: "FRItems",
                column: "FRItem_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FRItems_FRItems_FRItem_Id",
                table: "FRItems",
                column: "FRItem_Id",
                principalTable: "FRItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FRItems_FRItems_FRItem_Id",
                table: "FRItems");

            migrationBuilder.DropIndex(
                name: "IX_FRItems_FRItem_Id",
                table: "FRItems");

            migrationBuilder.DropColumn(
                name: "FRItem_Id",
                table: "FRItems");
        }
    }
}
