using ExaminationProject.Data;
using ExaminationProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Areas.Dashboard.Controllers
{
    [Area(nameof(Dashboard))]
    public class AnswerController : Controller
    {
        private readonly AppDbContext _context;

        public AnswerController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var answer = await _context.Answers.ToListAsync();
            return View(answer);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Answer answer)
        {
            try
            {
                _context.Answers.Add(answer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var edit = _context.Answers.FirstOrDefault(e => e.Id == id);
            _context.SaveChanges();
            return View(edit);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Answer answer)
        {
            _context.Answers.Update(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var answer = await _context.Answers.SingleOrDefaultAsync(x => x.Id == id);
            return View(answer);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Answer answer)
        {
            _context.Answers.RemoveRange(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
