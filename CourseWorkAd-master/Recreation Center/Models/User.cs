using System.ComponentModel.DataAnnotations;

namespace CourseWorkAd.Models
{
    public class User
    {
        [Key]
        public int UserNumber { get; set; }
        public string? UserName { get; set; }
        public string? UserType { get; set; }
        public string? password { get; set; }
    }
}
