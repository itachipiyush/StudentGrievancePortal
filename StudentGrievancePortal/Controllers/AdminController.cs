using Microsoft.AspNetCore.Mvc;
using StudentGrievancePortal.Models;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace StudentGrievancePortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly GrievanceContext _context;

        public AdminController(GrievanceContext context)
        {
            _context = context;
        }

        public IActionResult Summary()
        {
            var roleId = HttpContext.Session.GetInt32("UserRole");
            var deptId = HttpContext.Session.GetInt32("UserDept");

            if (roleId == null || roleId != 2)
                return RedirectToAction("Login", "Account");

            var total = _context.Grievances.Count(g => g.AssignedDeptId == deptId);
            var resolved = _context.Grievances.Count(g => g.Status == "Resolved" && g.AssignedDeptId == deptId);
            var pending = _context.Grievances.Count(g => (g.Status == "Submitted" || g.Status == "Under Review") && g.AssignedDeptId == deptId);

            var recentGrievances = _context.Grievances
                .Where(g => g.AssignedDeptId == deptId)
                .OrderByDescending(g => g.CreatedAt)
                .Take(5)
                .ToList();

            ViewBag.Total = total;
            ViewBag.Resolved = resolved;
            ViewBag.Pending = pending;
            ViewBag.Recent = recentGrievances;

            return View();
        }

        public IActionResult ExportToCSV()
        {
            var deptId = HttpContext.Session.GetInt32("UserDept");
            if (deptId == null) return RedirectToAction("Login", "Account");

            var data = _context.Grievances
                .Where(g => g.AssignedDeptId == deptId)
                .OrderByDescending(g => g.CreatedAt)
                .ToList();

            var builder = new StringBuilder();
            builder.AppendLine("Ticket Number,Subject,Status,Priority,Date Submitted,Resolution Details");

            foreach (var item in data)
            {
                string resolution = item.ResolutionDetails?.Replace("\"", "'") ?? "No resolution provided";
                builder.AppendLine($"{item.TicketNumber},{item.Subject},{item.Status},{item.Priority},{item.CreatedAt:yyyy-MM-dd},\"{resolution}\"");
            }

            string fileName = $"Grievance_Report_{DateTime.Now:yyyyMMdd}.csv";
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", fileName);
        }
    }
}