using System.ComponentModel.DataAnnotations;

namespace CourseWorkAd.Models
{
    public class DVDCategory
    {
        [Key]
        public int CategoryNumber { get; set; } 
        public string? CategoryDescription { get; set; }

        public int AgeRestricted { get; set; }

        public ICollection<DVDTitle> DVDTitles { get; set; }
    }
}
