using ExaminationProject.Data;
using ExaminationProject.Models;
using ExaminationProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ExaminationProject.Controllers
{
    //[Authorize(Policy = "IsNotDeletedPolicy")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            HomeVM vm = new()
            {
                ExamCategories = _context.ExamCategories.Where(x => x.IsDeleted == false).ToList(),
            };
            
            return View(vm);
        }

        [HttpGet("examcategory/{id}")]
        public IActionResult ExamCategory(int id)
        {
            var questions = _context.Questions.Where(x => x.ExamCategoryId == id).ToList();
            var questionIds = questions.Select(x => x.Id).ToList();
            var questionAnswers = _context.QuestionAnswers
                .Include(x => x.Answer)
                .Where(x => questionIds.Contains(x.QuestionId))
                .ToList();
            var answers = questionAnswers.Select(x => x.Answer).Distinct().ToList();

            var viewModel = new ExamCategoryVM
            {
                SelectedCategoryId = id,
                Questions = questions,
                Answers = answers,
                QuestionAnswers = questionAnswers
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}