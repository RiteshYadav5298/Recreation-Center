using CourseWorkAd.DBContext;
using CourseWorkAd.Models;
using CourseWorkAd.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CourseWorkAd.Controllers
{

    public class UserController : Controller
    {
        private readonly ApplicationDBContext dbContext;
        public UserController(ApplicationDBContext db)
        {
            dbContext = db;
        }

        public IActionResult Dashboard(bool IsLogin = false)

        {
            if (IsLogin)
            {
                var stock = dbContext.DVDCopy.ToArray();
                if (stock.Length != 0)
                {
                    ViewBag.stockData = "hasValue";
                    ViewBag.books = stock;
                }
                else
                {
                    ViewBag.stockData = "null";
                }
            }
            ViewBag.isLogin = IsLogin;
            return View();
        }
        public IActionResult EditUserPassword()
        {
            return View();
        }

        public IActionResult Password(bool Isnotchange = false)
        {
            ViewBag.isnotchange = Isnotchange;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditUserPassword(User users, string oldPassword, string newPassword)
        {
            var claimsPrinciple = User.Claims;
            var c = claimsPrinciple.Where(a => a.Type == "UserNumber").FirstOrDefault().Value;

            users.UserName = claimsPrinciple.Where(a => a.Type == "UserName").FirstOrDefault().Value;
            users.UserType = claimsPrinciple.Where(a => a.Type == "UserType").FirstOrDefault().Value;
            var oldpassword = claimsPrinciple.Where(a => a.Type == "password").FirstOrDefault().Value;
            users.password = newPassword;
            if (oldpassword == oldPassword)
            {
                try
                {

                    dbContext.Users.Update(users);
                    await dbContext.SaveChangesAsync();
                    return Redirect("~/User/Dashboard");
                }
                catch
                {
                    return RedirectToAction("Password", new { Isnotchange = true });
                }
            }
            else
            {
                return RedirectToAction("Password", new { Isnotchange = true });
            }
        }

        public IActionResult InactiveDVD()
        {
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly lastDate = currentDate.AddDays(-31);
            var inactiveItemList = dbContext.DVDCopy.ToArray();
            var data = from a in dbContext.Loans
                       from b in dbContext.DVDCopy
                       from c in dbContext.DVDTitles
                       where (a.CopyNumber == b.CopyNumber && a.DateOut <= lastDate && b.DVDNumber == c.DVDNumber)
                       select c;


            return View(data);
        }

        public IActionResult InactiveMembers()
        {
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly lastDate = currentDate.AddDays(-31);
            var inactiveItemList = from a in dbContext.Members
                                   from b in dbContext.Loans
                                   where (a.MemberNumber == b.MemberNumber && b.DateOut <= lastDate)
                                   select a;
            ViewBag.loans = dbContext.Loans.ToArray();
            ViewBag.copy = dbContext.DVDCopy.ToArray();
            ViewBag.title = dbContext.DVDTitles.ToArray();
            return View(inactiveItemList);

        }

        public IActionResult OnLoanData()
        {
            var loanData = dbContext.Loans.Where(a => a.DateReturned == null).OrderBy(x => x.DateOut).ToArray();
            ViewBag.dvd = dbContext.DVDTitles.OrderBy(a => a.DVDtitle).ToArray();
            ViewBag.copy = dbContext.DVDCopy.ToArray();
            ViewBag.mem = dbContext.Members.ToArray();
            var result = (from a in dbContext.Loans
                          group a.DateOut by a.DateDue into grp
                          select new Totaldayloanmodel { Dates = grp.Key, Count = grp.Count() }).ToList()
            ;

            ViewBag.re = result;
            return View(loanData);
        }

        public IActionResult OldDVD()
        {
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly lastDate = currentDate.AddYears(-1);
            var inactiveItemList = from a in dbContext.DVDTitles
                                   from b in dbContext.DVDCopy
                                   from c in dbContext.Loans
                                   where (c.DateReturned != null && c.DateReturned <= currentDate && b.CopyNumber == c.CopyNumber && b.DVDNumber == a.DVDNumber && a.DateReleased <= lastDate)
                                   select a;

            return View(inactiveItemList);
        }



        public IActionResult DeleteAllOldDVDS()
        {
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly lastDate = currentDate.AddYears(-1);
            var inactiveItem = dbContext.DVDTitles.Where(x => x.DateReleased <= lastDate).ToArray();
            foreach (var dvds in inactiveItem)
            {
                var inactivecopy = dbContext.DVDCopy.Where(x => x.DVDNumber == dvds.DVDNumber).First();
                var loandata = dbContext.Loans.Where(a => a.CopyNumber == inactivecopy.CopyNumber).First();
                if (loandata.DateReturned != null && loandata.DateReturned <= currentDate)
                {
                    dbContext.Remove(dvds);
                    dbContext.SaveChanges();
                }

            }
            return Redirect("Dashboard");
        }
        [HttpGet]
        public IActionResult DVDCastDetails()
        {
            ViewBag.cast = dbContext.CastMember.ToArray();
            ViewBag.actor = dbContext.Actors.OrderBy(b => b.ActorSurname).ToArray();
            ViewBag.producer = dbContext.Producers.ToArray();
            ViewBag.title = dbContext.DVDTitles.OrderBy(a => a.DateReleased).ToArray();
            ViewBag.stud = dbContext.Studio.ToArray();

            return View();
        }

        [HttpGet]

        public IActionResult lastDVDData()
        {
            ViewBag.listss = dbContext.DVDCopy.ToArray();
            ViewBag.titlelists = dbContext.DVDTitles.ToArray();
            ViewBag.memberlists = dbContext.Members.ToArray();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> lastDVDData(int CopyNumber)
        {
            ViewBag.listss = dbContext.DVDCopy.ToArray();
            ViewBag.titlelists = dbContext.DVDTitles.ToArray();
            ViewBag.memberlists = dbContext.Members.ToArray();
            var details = dbContext.Loans.Where(a => a.CopyNumber == CopyNumber).OrderBy(a => a.DateOut).LastOrDefault();

            return View(details);
        }

        public IActionResult Toomuchloan()
        {
            var result = (from a in dbContext.Loans
                          from b in dbContext.Members

                          where (a.DateReturned == null && a.MemberNumber == b.MemberNumber)
                          group a.MemberNumber by a.MemberNumber into grp
                          select new LoanCountViewModel
                          {
                              Name = grp.Key,
                              Count = grp.Count()
                          }).ToList();
            ;
            var member = dbContext.MembershipCategories.ToList();
            var members = dbContext.Members.ToList();
            //var da = result.Select(a=> new loan { a.name ,a.countt}).ToList();

            foreach (var d in result)
            {

                foreach (var c in member)
                {
                    var ad = dbContext.Members.Where(x => x.MemberNumber == d.Name).FirstOrDefault();

                    if (ad.MembershipCategoryNumber == c.MembershipCategoryNumber)
                    {
                        ViewBag.de = member;
                        ViewBag.d = result;
                        return View(members);
                    }
                    else
                    {
                        return View();
                    }
                }

            }
            return View();
        }


    }
}

