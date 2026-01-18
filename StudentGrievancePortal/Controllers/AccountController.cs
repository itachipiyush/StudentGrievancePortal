//using Microsoft.AspNetCore.Mvc;
//using StudentGrievancePortal.Models;
//using System.Linq;

//namespace StudentGrievancePortal.Controllers
//{
//    public class AccountController : Controller
//    {
//        private readonly GrievanceContext _context;

//        public AccountController(GrievanceContext context)
//        {
//            _context = context;
//        }

//        // GET: /Account/Login
//        public IActionResult Login()
//        {
//            return View();
//        }

//        // POST: /Account/Login
//        [HttpPost]
//        public IActionResult Login(string erpId, string password)
//        {
//            // In a real ERP, you'd use password hashing. For this project, we check plain text or simulated hash.
//            var user = _context.Users.FirstOrDefault(u => u.ERP_Id == erpId && u.PasswordHash == password);

//            if (user != null)
//            {
//                // Store essential user info in Session
//                HttpContext.Session.SetInt32("UserId", user.UserId);
//                HttpContext.Session.SetString("UserName", user.FullName);
//                HttpContext.Session.SetInt32("UserRole", user.RoleId);
//                HttpContext.Session.SetInt32("UserDept", user.DeptId);

//                if (user.RoleId == 1) // Student
//                    return RedirectToAction("Index", "Student");
//                else if (user.RoleId == 2) // Coordinator
//                    return RedirectToAction("Dashboard", "Coordinator");
//            }

//            ViewBag.Error = "Invalid ERP ID or Password";
//            return View();
//        }

//        public IActionResult Logout()
//        {
//            HttpContext.Session.Clear();
//            return RedirectToAction("Login");
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using StudentGrievancePortal.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace StudentGrievancePortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly GrievanceContext _context;

        public AccountController(GrievanceContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            // If user is already logged in, redirect them to their dashboard
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                int? roleId = HttpContext.Session.GetInt32("UserRole");
                if (roleId == 1) return RedirectToAction("Index", "Student");
                if (roleId == 2) return RedirectToAction("Dashboard", "Coordinator");
            }
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(string erpId, string password, int userRole)
        {
            // Verification logic: Match ERP_Id, Password, and the Selected Role
            var user = _context.Users.FirstOrDefault(u =>
                u.ERP_Id == erpId &&
                u.PasswordHash == password &&
                u.RoleId == userRole);

            if (user != null)
            {
                // Store essential user info in Session
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("UserName", user.FullName);
                HttpContext.Session.SetInt32("UserRole", user.RoleId);
                HttpContext.Session.SetInt32("UserDept", user.DeptId);

                // Role-based redirection
                if (user.RoleId == 1) // Student
                {
                    return RedirectToAction("Index", "Student");
                }
                else if (user.RoleId == 2) // Coordinator
                {
                    return RedirectToAction("Dashboard", "Coordinator");
                }
            }

            // If login fails, provide a specific error message
            ViewBag.Error = "Invalid Credentials or Role Selection. Please try again.";
            return View();
        }

        public IActionResult Logout()
        {
            // Clear all session data to log out the user
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}