using System.ComponentModel.DataAnnotations;

namespace CourseWorkAd.Models
{
    public class LoanType
    {
        [Key]
        public int LoanTypeNumber { get; set; }
        public string Loantype { get; set; }

        public int LoanDuration { get; set; }
        public ICollection<Loan> Loans { get; set; }
    }
}
