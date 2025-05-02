using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseWorkAd.Models
{
    public class DVDTitle
    {
        [Key]
        public int DVDNumber { get; set; }

        public int CategoryNumber { get; set; }
        [ForeignKey ("CategoryNumber")]
        public DVDCategory Category { get; set; }

        public int StudioNumber { get; set; }
        [ForeignKey ("StudioNumber")]

        public Studio Studio { get; set; }

        public int ProducerNumber { get; set; }
        [ForeignKey ("ProducerNumber")]
        public Producer Producer { get; set; }

        public string? DVDtitle { get; set; }

        public DateOnly DateReleased { get; set; }

        public int StandardCharge { get; set; }

        public int PenaltyCharge { get; set; }

        public ICollection<DVDCopy> DVDCopy { get; set; }



    }
}
