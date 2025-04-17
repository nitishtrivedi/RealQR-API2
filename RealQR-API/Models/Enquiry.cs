using System.ComponentModel.DataAnnotations;

namespace RealQR_API.Models
{
    public class Enquiry
    {
        [Key]
        public int Id { get; set; }

        //Essential Information
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string MethodOfContact { get; set; } //Phone or email

        //Property Information
        public string BudgetRange { get; set; }
        public string PreferredAreas { get; set; }
        public string PropertyType { get; set; }
        public string ModeOfPayment { get; set; } //Loan, self funding etc.
        public string PurchaseTimeFrame { get; set; }
        public string PurchaseType { get; set; } //Flat, bungalow etc.
        public string OtherQuestions { get; set; }
        public bool ConsentToCall { get; set; }
    }
}
