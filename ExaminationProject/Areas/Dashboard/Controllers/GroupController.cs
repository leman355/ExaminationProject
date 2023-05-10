using ExaminationProject.Data;
using ExaminationProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExaminationProject.Areas.Dashboard.Controllers
{

    [Area("Dashboard")]
    public class GroupController : Controller
    {
        private readonly AppDbContext _context;

        public GroupController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var group = _context.Groups.ToList();
            return View(group);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Group group)
        {
            try
            {
                _context.Groups.Add(group);
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
            var edit = _context.Groups.FirstOrDefault(e => e.Id == id);
            _context.SaveChanges();
            return View(edit);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Group group)
        {

            try
            {
                group.UpdatedDate = DateTime.Now;
                _context.Groups.Update(group);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var del = await _context.Groups.SingleOrDefaultAsync(x => x.Id == id);
            return View(del);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Group group)
        {
            try
            {
                var gr = await _context.Groups.SingleOrDefaultAsync(x => x.Id == group.Id);
                gr.IsDeleted = true;
                _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
