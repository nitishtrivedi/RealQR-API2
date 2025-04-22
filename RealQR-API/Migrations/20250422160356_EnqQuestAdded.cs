using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealQR_API.Migrations
{
    /// <inheritdoc />
    public partial class EnqQuestAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnquiryStatus",
                table: "Enquiry");

            migrationBuilder.CreateTable(
                name: "EnquiryQuestionnaire",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnquiryId = table.Column<int>(type: "int", nullable: false),
                    EnquiryStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasConfirmedBudget = table.Column<bool>(type: "bit", nullable: false),
                    RefinedBudgetRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReconfirmModeOfPayment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoanProcessingConsent = table.Column<bool>(type: "bit", nullable: false),
                    LoanProcessingVendor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FollowUpActions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnquiryQuestionnaire", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnquiryQuestionnaire_Enquiry_EnquiryId",
                        column: x => x.EnquiryId,
                        principalTable: "Enquiry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnquiryQuestionnaire_EnquiryId",
                table: "EnquiryQuestionnaire",
                column: "EnquiryId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnquiryQuestionnaire");

            migrationBuilder.AddColumn<string>(
                name: "EnquiryStatus",
                table: "Enquiry",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
