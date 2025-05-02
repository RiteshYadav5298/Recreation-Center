using System.ComponentModel.DataAnnotations;

namespace CourseWorkAd.Models
{
    public class Producer
    {
        [Key]
        public int ProducerNumber { get; set; } 
        public string ProducerName { get; set; }

        public ICollection<DVDTitle> DVDTitles { get; set; }
    }
}
