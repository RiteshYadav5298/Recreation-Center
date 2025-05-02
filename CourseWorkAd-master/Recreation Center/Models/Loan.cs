using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseWorkAd.Models
{
    public class Loan
    {
        [Key]
        public int LoanNumber { get; set; }
        public int LoanTypeNumber { get; set; }
        [ForeignKey("LoanTypeNumber")]
        public LoanType LoanType { get; set; }

        public int CopyNumber { get; set; }
        [ForeignKey("CopyNumber")]
        public DVDCopy DVDCopy { get; set; }
        
        
        public int MemberNumber { get; set; }
        [ForeignKey("MemberNumber")]

        public Member Member { get; set; }
        public DateOnly DateOut { get; set; }
        public DateOnly DateDue { get; set; }

        public DateOnly? DateReturned { get; set; }

        


    }
}
