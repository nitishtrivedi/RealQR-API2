namespace RealQR_API.DTOs
{
    public class EnquiryDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string MethodOfContact { get; set; }
        public string BudgetRange { get; set; }
        public string PreferredAreas { get; set; }
        public string PropertyType { get; set; }
        public string ModeOfPayment { get; set; }
        public string PurchaseTimeFrame { get; set; }
        public string PurchaseType { get; set; }
        public string OtherQuestions { get; set; }
        public bool ConsentToCall { get; set; }
        public EnquiryQuestionnaireDto EnquiryQuestionnaire { get; set; }
    }

    public class EnquiryQuestionnaireDto
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public string EnquiryStatus { get; set; }
        public string AgentName { get; set; }
        public bool HasConfirmedBudget { get; set; }
        public string RefinedBudgetRange { get; set; }
        public string ReconfirmModeOfPayment { get; set; }
        public bool LoanProcessingConsent { get; set; }
        public string LoanProcessingVendor { get; set; }
        public string FollowUpActions { get; set; }
        public string Comments { get; set; }
    }
}
