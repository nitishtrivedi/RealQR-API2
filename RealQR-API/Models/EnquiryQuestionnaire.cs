using System.ComponentModel.DataAnnotations;

namespace RealQR_API.Models
{
    public class EnquiryQuestionnaire
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int EnquiryId { get; set; }
        //public Enquiry Enquiry { get; set; }
        public string EnquiryStatus { get; set; } = "Open"; // Same like the one in initial enquiry
        public string AgentName { get; set; } // Name who is acting on the enquiry
        public bool HasConfirmedBudget { get; set; } //Has customer confirmed the budget they entered in the initial form
        public string RefinedBudgetRange { get; set; } //If not above, ask them again their exact budget
        public string ReconfirmModeOfPayment { get; set; } //Reconfirm with the customer their MOP
        public bool LoanProcessingConsent { get; set; } //If MOP is Loan, is customer allowing us to process their loan?
        public string LoanProcessingVendor { get; set; } //If not us, then who is their loan provider
        public string FollowUpActions { get; set; } //Decide a few follow up actions to manage this enquiry
        public string Comments { get; set; } //Detailed comments of the agent about the conversation with the customer

    }
}
