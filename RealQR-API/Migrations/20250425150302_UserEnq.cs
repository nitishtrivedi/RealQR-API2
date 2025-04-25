using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealQR_API.Migrations
{
    /// <inheritdoc />
    public partial class UserEnq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Enquiry",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enquiry_UserId",
                table: "Enquiry",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enquiry_Users_UserId",
                table: "Enquiry",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enquiry_Users_UserId",
                table: "Enquiry");

            migrationBuilder.DropIndex(
                name: "IX_Enquiry_UserId",
                table: "Enquiry");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Enquiry");
        }
    }
}
