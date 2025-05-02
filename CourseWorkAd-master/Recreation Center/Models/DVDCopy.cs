using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseWorkAd.Models
{
    public class DVDCopy
    {
        [Key]
        public int CopyNumber { get; set; } 
        public int DVDNumber { get; set; }  
        public DateOnly DatePurchase { get; set; }
        [ForeignKey("DVDNumber")]
        public DVDTitle DVDTitles { get; set; }
        public ICollection<Loan> Loans { get; set; }

    }
}
