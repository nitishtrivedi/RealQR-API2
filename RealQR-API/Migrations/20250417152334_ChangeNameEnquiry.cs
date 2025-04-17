using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealQR_API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameEnquiry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Form");

            migrationBuilder.CreateTable(
                name: "Enquiry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MethodOfContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BudgetRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreferredAreas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModeOfPayment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseTimeFrame = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherQuestions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConsentToCall = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enquiry", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enquiry");

            migrationBuilder.CreateTable(
                name: "Form",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConsentToCall = table.Column<bool>(type: "bit", nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MethodOfContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModeOfPayment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherQuestions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreferredAreas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropertyType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseTimeFrame = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Form", x => x.Id);
                });
        }
    }
}
