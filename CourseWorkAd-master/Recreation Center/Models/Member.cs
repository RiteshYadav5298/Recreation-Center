using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseWorkAd.Models
{
    public class Member
    {
        [Key]
        public int MemberNumber { get; set; }
        public int MembershipCategoryNumber { get; set; }
        [ForeignKey("MembershipCategoryNumber")]
        public MembershipCategory MembershipCategories { get; set; }
        public string MemberLastName { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberAddress { get; set; }
        public DateOnly MemberDateOfBirth { get; set; }
    }
}
