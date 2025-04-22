using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealQR_API.Migrations
{
    /// <inheritdoc />
    public partial class AddingEnqStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EnquiryStatus",
                table: "Enquiry",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnquiryStatus",
                table: "Enquiry");
        }
    }
}
