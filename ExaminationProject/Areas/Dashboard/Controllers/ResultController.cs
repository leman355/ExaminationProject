using ExaminationProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExaminationProject.Areas.Dashboard.Controllers
{
    [Area(nameof(Dashboard))]
    public class ResultController : Controller
    {
        private readonly AppDbContext _context;
        public ResultController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var result = _context.ExamResults.Include(x => x.User).Include(x => x.ExamCategory).ToList();
            return View(result);
        }
    }
}
