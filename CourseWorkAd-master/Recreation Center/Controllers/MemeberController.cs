using CourseWorkAd.DBContext;
using Microsoft.AspNetCore.Mvc;

namespace CourseWorkAd.Controllers
{
    public class MemberController : Controller
    {
        private readonly ApplicationDBContext dbContext;
        public MemberController(ApplicationDBContext db)
        {
            dbContext = db;
        }
        public IActionResult PurchaseHistoryDatas()
        {
            return View();
        }

        public IActionResult Home()
        {
            return View();
        }

        /*[Route("login/{email:string}/{password:string}")]*/
        [Route("login/{email:int}/{password:int}")]
        public IActionResult Login(int email, int password)
        {
            return new ContentResult { Content = string.Format("Email: {0}; Passsword: {1};", email, password) };
        }
        [HttpGet]

        public IActionResult ProfileData(int MemberNumber)
        {
            ViewBag.list = dbContext.Members.ToArray();

            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly lastDate = currentDate.AddDays(-31);
            var userHistory = from a in dbContext.Loans
                              from b in dbContext.DVDCopy
                              from c in dbContext.DVDTitles
                              where (a.MemberNumber == MemberNumber && a.DateOut >= lastDate && a.CopyNumber == b.CopyNumber && b.DVDNumber == c.DVDNumber)
                              select c;

            ViewBag.copy = dbContext.DVDCopy.ToArray();
            return View(userHistory);
        }


    }
}
