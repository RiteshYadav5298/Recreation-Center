using CourseWorkAd.DBContext;
using CourseWorkAd.Models;
using Microsoft.AspNetCore.Mvc;

namespace CourseWorkAd.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDBContext  dbContext;

        public AdminController(ApplicationDBContext db)
        {
            dbContext = db;
        }
        [HttpGet]

        public IActionResult Home(bool Iscustadd = false, bool Isdeletecust = false, bool Isdeleteuser = false, bool Isadduser = false, bool Islogin = false)
        {
            ViewBag.islogin = Islogin;
            ViewBag.iscustadd = Iscustadd;
            ViewBag.isdeletecust = Isdeletecust;
            ViewBag.isdeleteuser = Isdeleteuser;
            ViewBag.isadduser = Isadduser;
            //logged in token data retrieve
            var users = new User
            {
                UserNumber = 1,
                UserName = "Admin",
                UserType = "admin",

            };
            return View(users);
        }

        public IActionResult Users()
        {
            return RedirectToAction("Home", "User");

        }
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User users, string Username, string usertype, string password)
        {
            users.UserName = Username;
            users.UserType = usertype;
            users.password = password;
            try
            {
                dbContext.Users.Add(users);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Home", new { Isadduser = true });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IActionResult AddMembers()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMembers(Member members, string memberFirstName, string memberLastName, string memberAddress, string category, DateTime DateOfBirth)
        {
            members.MemberFirstName = memberFirstName;
            members.MemberLastName = memberLastName;
            members.MemberAddress = memberAddress;
            members.MemberDateOfBirth = DateOnly.FromDateTime(DateOfBirth);
            var membership = dbContext.MembershipCategories.Where(x => x.MembershipCategoryDescription == category).FirstOrDefault();
            members.MembershipCategoryNumber = membership.MembershipCategoryNumber;
            try
            {
                dbContext.Members.Add(members);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Home", new { Iscustadd = true });
            }
            catch (Exception)
            {
                return null;
            }
        }
        public IActionResult ManageUsers()
        {
            var userDetails = dbContext.Users.ToArray();
            return View(userDetails);
        }

        //to delete user from the data table
        public IActionResult DeleteUsers(int UserNumber)
        {
            var users = dbContext.Users.Where(x => x.UserNumber == UserNumber).FirstOrDefault();
            dbContext.Users.Remove(users);
            dbContext.SaveChanges();
            return RedirectToAction("Home", new { Isdeleteuser = true });
        }


        public async Task<IActionResult> EditUsers(User users, string Username, string usertype, string password)
        {
            users.UserName = Username;
            users.UserType = usertype;
            users.password = password;

            try
            {
                dbContext.Users.Update(users);
                await dbContext.SaveChangesAsync();
                return Redirect("~/Admin/ManageUsers");
            }
            catch
            {
                return null;
            }

        }

        public IActionResult UpdateUsers(int UserNumber)
        {
            ViewBag.user_data = dbContext.Users.Where(x => x.UserNumber == UserNumber).FirstOrDefault();
            return View();
        }

       
        public IActionResult ManageMembers()
        {
           
            var memberDetails = dbContext.Members.ToArray();
            return View(memberDetails);
        }

       
        public IActionResult DeleteMembers(int MemberNumber)
        {
            var members_datas = dbContext.Members.Where(x => x.MemberNumber == MemberNumber).FirstOrDefault();
            dbContext.Members.Remove(members_datas);
            dbContext.SaveChanges();
            return RedirectToAction("Home", new { Isdeletecust = true });
        }

        public async Task<IActionResult> EditMembers(Member members, string memberFirstName, string memberLastName, string memberAddress, DateTime DateOfBirth, string category)
        {
            members.MemberFirstName = memberFirstName;
            members.MemberLastName = memberLastName;
            members.MemberAddress = memberAddress;
            members.MemberDateOfBirth = DateOnly.FromDateTime(DateOfBirth);
            var membership = dbContext.MembershipCategories.Where(x => x.MembershipCategoryDescription == category).FirstOrDefault();
            members.MembershipCategoryNumber = membership.MembershipCategoryNumber;
            try
            {
                dbContext.Members.Update(members);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("~/Admin/ManageMembers");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IActionResult UpdateMembers(int MemberNumber)
        {
            ViewBag.member_data = dbContext.Members.Where(x => x.MemberNumber == MemberNumber).First();
            return View();
        }
       
        public IActionResult Categories(bool Issuccess = false, bool Isdeletecate = false)
        {
            ViewBag.issuccess = Issuccess;
            ViewBag.isdeletecate = Isdeletecate;
            var categoryLists = dbContext.MembershipCategories.ToArray();
            return View(categoryLists);
        }

        public IActionResult AddMemCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMemCategory(MembershipCategory category, string categorydes, int loan)
        {
            category.MembershipCategoryDescription = categorydes;
            category.MembershipCategoryTotalLoans = loan;

            try
            {
                dbContext.MembershipCategories.Add(category);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("AddMemCategory", new { Issuccess = true });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IActionResult DeleteMemCategory(int MembershipCategoryNumber)
        {
            var category_data = dbContext.MembershipCategories.Where(x => x.MembershipCategoryNumber == MembershipCategoryNumber).FirstOrDefault();
            dbContext.MembershipCategories.Remove(category_data);
            dbContext.SaveChanges();
            return RedirectToAction("AddMemCategory", new { Isdeletecate = true });
        }
    }

}
