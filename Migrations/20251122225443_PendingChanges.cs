using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace COMP2139___assignment2.Migrations
{
    /// <inheritdoc />
    public partial class PendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizerIdStr",
                table: "Events");

            migrationBuilder.AlterColumn<string>(
                name: "OrganizerId",
                table: "Events",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Events_OrganizerId",
                table: "Events",
                column: "OrganizerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_OrganizerId",
                table: "Events",
                column: "OrganizerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_OrganizerId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_OrganizerId",
                table: "Events");

            migrationBuilder.AlterColumn<int>(
                name: "OrganizerId",
                table: "Events",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "OrganizerIdStr",
                table: "Events",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
