using CourseWorkAd.DBContext;
using CourseWorkAd.Models;
using Itenso.TimePeriod;
using Microsoft.AspNetCore.Mvc;

namespace CourseWorkAd.Controllers
{
    public class LoanController : Controller
    {
        private readonly ApplicationDBContext dbContext;

        public LoanController(ApplicationDBContext db)
        {
            dbContext = db;
        }

        public IActionResult List()
        {
            var salelist = dbContext.Loans.ToArray();
            return View(salelist);
        }

        public IActionResult Items(int CopyNumber)
        {
            var Itemlist = dbContext.DVDCopy.Where(x => x.CopyNumber == CopyNumber).ToArray();
            return View(Itemlist);
        }
        [HttpGet]
        public IActionResult ReturnDVD()
        {
            ViewBag.memberList = dbContext.Members.ToArray();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ReturnDVD(Loan loan, int member, int copy)
        {
            ViewBag.memberList = dbContext.Members.ToArray();
            var loans = dbContext.Loans.Where(x => x.CopyNumber == copy && x.MemberNumber == member && x.DateReturned == null).FirstOrDefault();
            ViewBag.total = Paid(loans.LoanNumber);
            DateDiff dateDiff = new DateDiff(DateTime.Now, loans.DateDue.ToDateTime(TimeOnly.Parse("10:00 PM")));
            int q = dateDiff.Days;
            try
            {
                var de = from a in dbContext.Loans
                         from b in dbContext.DVDCopy
                         from c in dbContext.DVDTitles
                         where (c.DVDNumber == b.DVDNumber && b.CopyNumber == a.CopyNumber && a.LoanNumber == loans.LoanNumber && a.DateDue >= DateOnly.FromDateTime(DateTime.Now))
                         select c.PenaltyCharge;
                var am = de.FirstOrDefault();
                ViewBag.Penalty = q * am;
                var lo = dbContext.Loans.Where(x => x.LoanNumber == loans.LoanNumber).FirstOrDefault();
                loan.DateReturned = DateOnly.FromDateTime(DateTime.Now);
                try
                {
                    dbContext.Loans.Update(loan);
                    await dbContext.SaveChangesAsync();
                    return View(loans);

                }
                catch
                {
                    return View();
                }

            }
            catch
            {
                /*var lo = dbContext.Loans.Where(x => x.LoanNumber == loandata.LoanNumber).FirstOrDefault();*/
                loans.DateReturned = DateOnly.FromDateTime(DateTime.Now);
                try
                {
                    dbContext.Loans.Update(loans);
                    await dbContext.SaveChangesAsync();
                    return View(loans);

                }
                catch
                {
                    return View();
                }
            }


        }


        public async Task<IActionResult> Return(Loan loan, int LoanNumber)
        {
            var loans = dbContext.Loans.Where(x => x.LoanNumber == LoanNumber).FirstOrDefault();
            loan.DateReturned = DateOnly.FromDateTime(DateTime.Now);
            try
            {
                dbContext.Loans.Update(loan);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("ReturnDVD");
            }
            catch
            {
                return View();
            }
        }
       
        public IActionResult AddLoans()
        {
            ViewBag.memberList = dbContext.Members.ToArray();

            ViewBag.loantypeList = dbContext.LoanTypes.ToArray();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddLoans(Loan loans, int member_number, int loantype, int copynum)
        {
            loans.DateOut = DateOnly.FromDateTime(DateTime.Now);

            loans.MemberNumber = member_number;
            loans.LoanTypeNumber = loantype;
            var lo = dbContext.LoanTypes.Where(a => a.LoanTypeNumber == loantype).FirstOrDefault();
            var dat = lo.LoanDuration;
            var mem = dbContext.Members.Where(x => x.MemberNumber == member_number).FirstOrDefault();
            loans.DateDue = DateOnly.FromDateTime(DateTime.Now).AddDays(dat);
            loans.CopyNumber = copynum;
            var age = from a in dbContext.DVDCategories
                      from b in dbContext.DVDTitles
                      from c in dbContext.DVDCopy
                      where (a.CategoryNumber == b.CategoryNumber && b.DVDNumber == c.DVDNumber && c.CopyNumber == copynum)
                      select a.AgeRestricted;
            var f = age.FirstOrDefault();
            DateDiff dateDiff = new DateDiff(DateTime.Now, mem.MemberDateOfBirth.ToDateTime(TimeOnly.Parse("10:00 PM")));
            try
            {
                if ((dateDiff.Years) >= f)
                {
                    return Content("Not Available");
                }
                else
                {
                    dbContext.Loans.Add(loans);
                    await dbContext.SaveChangesAsync();
                    var loanData = dbContext.Loans.ToArray();
                    var data = loanData.LastOrDefault();
                    return RedirectToAction("Pay", new { member = member_number, copy = copynum });
                }
            }
            catch (Exception)
            {
                return View();
            }
        }
       
        

       
        public IActionResult Addloantype(bool Issuccess = false, bool Isdeletetype = false)
        {
            ViewBag.issuccess = Issuccess;
            ViewBag.isdeletetype = Isdeletetype;
            ViewBag.typeList = dbContext.LoanTypes.ToArray();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Addloantype(LoanType loans, string loantype, int durat)
        {
            loans.Loantype = loantype;
            loans.LoanDuration = durat;

            try
            {
                dbContext.LoanTypes.Add(loans);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("AddLoanType", new { Issuccess = true });
            }
            catch (Exception)
            {
                return View();
            }
        }

        public IActionResult Deleteloantype(int LoanTypeNumber)
        {
            var loandata = dbContext.LoanTypes.Where(x => x.LoanTypeNumber == LoanTypeNumber).First();
            dbContext.LoanTypes.Remove(loandata);
            dbContext.SaveChanges();
            return RedirectToAction("AddLoanType", new { Isdeletetype = true });
        }
        public IActionResult Pay(int member, int copy)
        {
            var data = dbContext.Loans.Where(a => a.MemberNumber == member && a.CopyNumber == copy).FirstOrDefault();
            var loans = dbContext.LoanTypes.Where(b => b.LoanTypeNumber == data.LoanTypeNumber).FirstOrDefault();

            var lo = loans.LoanDuration;
            var des = from a in dbContext.Loans
                     from b in dbContext.DVDCopy
                     from c in dbContext.DVDTitles
                     where (c.DVDNumber == b.DVDNumber && b.CopyNumber == a.CopyNumber && a.LoanNumber == data.LoanNumber)
                     select c.StandardCharge;
            var d = des.FirstOrDefault();
            var da = d * lo;
            ViewBag.stand = des;
            ViewBag.loandur = loans;
            ViewBag.pay = da;
            return View();
        }
        public int Paid(int loannumber)
        {
            var data = dbContext.Loans.Where(a => a.LoanNumber == loannumber).FirstOrDefault();
            var loan = from b in dbContext.LoanTypes
                      where (b.LoanTypeNumber == data.LoanTypeNumber)
                      select b.LoanDuration;
            var l = loan.FirstOrDefault();
            var des = from a in dbContext.Loans
                     from b in dbContext.DVDCopy
                     from c in dbContext.DVDTitles
                     where (c.DVDNumber == b.DVDNumber && b.CopyNumber == a.CopyNumber && a.LoanNumber == loannumber)
                     select c.StandardCharge;
            var d = des.FirstOrDefault();
            var da = d * l;
            return da;
        }

    }
}
