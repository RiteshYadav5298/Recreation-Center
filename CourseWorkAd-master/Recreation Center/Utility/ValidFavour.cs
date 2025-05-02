using CourseWorkAd.DBContext;
using CourseWorkAd.Models;
using System.Security.Claims;

namespace CourseWorkAd.Utility
{
    public class ValidFavour
    {
        private readonly ApplicationDBContext _context;
        public ValidFavour(ApplicationDBContext context)
        {
            _context = context;
        }

        internal User GetUserById(int id)
        {
            var appUser = _context.Users.Find(id);
            return appUser;
        }

        internal bool TryValidateUser(string username, string password, out List<Claim> claims)
        {
            claims = new List<Claim>();
            var appUser = _context.Users
                .Where(a => a.UserName == username)
                .Where(a => a.password == password).FirstOrDefault();
            if (appUser is null)
            {
                return false;
            }
            else
            {
                claims.Add(new Claim("UserNumber", appUser.UserNumber.ToString()));
                claims.Add(new Claim("UserName", appUser.UserName));
                claims.Add(new Claim("UserType", appUser.UserType));
                claims.Add(new Claim("password", appUser.password));
            }
            return true;
        }
    }
}
