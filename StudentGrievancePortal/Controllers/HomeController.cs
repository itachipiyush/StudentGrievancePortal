//using System.Diagnostics;
//using Microsoft.AspNetCore.Mvc;
//using StudentGrievancePortal.Models;

//namespace StudentGrievancePortal.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;

//        public HomeController(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace StudentGrievancePortal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var roleId = HttpContext.Session.GetInt32("UserRole");

            // Redirect logged-in users to their respective dashboards
            if (roleId == 1) return RedirectToAction("Index", "Student");
            if (roleId == 2) return RedirectToAction("Dashboard", "Coordinator");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}