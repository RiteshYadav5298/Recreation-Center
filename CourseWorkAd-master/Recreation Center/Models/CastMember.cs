using CourseWorkAd.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseWorkAd.Models
{
    public class CastMember
    {
        public int DVDNumber { get; set; }
        public int ActorNumber { get; set; }
        [ForeignKey("DVDNumber")]
        public DVDTitle DVDTitles { get; set; }
        [ForeignKey("ActorNumber")]
        public Actor Actor { get; set; }
    }
}
