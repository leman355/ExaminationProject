using ExaminationProject.Areas.Dashboard.ViewModels;
using ExaminationProject.Data;
using ExaminationProject.Models;
using ExaminationProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Claims;

namespace ExaminationProject.Controllers
{
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
        //[Authorize(Policy = "IsNotDeletedPolicy")]
        public IActionResult ExamCategory(int id)
        {
            var examCategory = _context.ExamCategories.Where(x => x.Id == id).FirstOrDefault();
            var questions = _context.Questions.Where(x => x.ExamCategoryId == id).Where(x => x.IsDeleted == false).ToList();
            var questionIds = questions.Select(x => x.Id).ToList();
            var questionAnswers = _context.QuestionAnswers
                .Include(x => x.Answer)
                .Where(x => questionIds.Contains(x.QuestionId))
                .ToList();
            var answers = questionAnswers.Select(x => x.Answer).Distinct().ToList();
            var correctAnswerIds = new List<int>();
            foreach (var question in questions)
            {
                var questionAnswersWithStatus = _context.QuestionAnswers
                    .Include(x => x.Answer)
                    .Where(x => x.QuestionId == question.Id && x.Answer.Status)
                    .ToList();

                correctAnswerIds.AddRange(questionAnswersWithStatus.Select(x => x.Answer.Id));
            }

            var viewModel = new ExamCategoryVM
            {
                SelectedCategoryId = id,
                SelectedCategoryName = examCategory.CategoryName,
                Questions = questions,
                Answers = answers,
                QuestionAnswers = questionAnswers,
                CorrectAnswerIds = correctAnswerIds,
            };
            return View(viewModel);
        }

        [HttpPost("examcategory/{id}")]
        public IActionResult ExamCategory(int id, List<int> selectedAnswerIds)
        {
            var examCategory = _context.ExamCategories.FirstOrDefault(x => x.Id == id);
            var questions = _context.Questions.Where(x => x.ExamCategoryId == id).ToList();
            var questionIds = questions.Select(x => x.Id).ToList();
            var questionAnswers = _context.QuestionAnswers
                .Include(x => x.Answer)
                .Where(x => questionIds.Contains(x.QuestionId))
                .ToList();

            // Обновляем значения свойства Selected для выбранных ответов
            foreach (var answer in questionAnswers.Select(x => x.Answer))
            {
                answer.Selected = selectedAnswerIds.Contains(answer.Id);
            }

            _context.SaveChanges();

            var answers = questionAnswers.Select(x => x.Answer).Distinct().ToList();

            var correctAnswerIds = new List<int>();
            foreach (var question in questions)
            {
                var questionAnswersWithStatus = _context.QuestionAnswers
                    .Include(x => x.Answer)
                    .Where(x => x.QuestionId == question.Id && x.Answer.Status)
                    .ToList();

                correctAnswerIds.AddRange(questionAnswersWithStatus.Select(x => x.Answer.Id));
            }

            var viewModel = new ExamCategoryVM
            {
                SelectedCategoryId = id,
                SelectedCategoryName = examCategory.CategoryName,
                Questions = questions,
                Answers = answers,
                QuestionAnswers = questionAnswers,
                SelectedAnswerIds = selectedAnswerIds,
                CorrectAnswerIds = correctAnswerIds,
            };

            int correctAnswersCount = viewModel.QuestionAnswers
               .Where(qa => qa.Answer.Status)
               .Count(qa => viewModel.SelectedAnswerIds.Contains(qa.Answer.Id));

            var examResult = new ExamResult
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                ExamCategoryId = viewModel.SelectedCategoryId,
                CorrectAnswers = correctAnswersCount,
                TotalQuestions = viewModel.Questions.Count(q => !q.IsDeleted),
                DateTaken = DateTime.Now,
            };
            _context.ExamResults.Add(examResult);
            _context.SaveChanges();

            return View("Result", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Result(ExamCategoryVM examCategoryVM)
        {
            //int correctAnswersCount = examCategoryVM.QuestionAnswers
            //    .Where(qa => qa.Answer.Status)
            //    .Count(qa => examCategoryVM.SelectedAnswerIds.Contains(qa.Answer.Id));

            //var examResult = new ExamResult
            //{
            //    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            //    ExamCategoryId = examCategoryVM.SelectedCategoryId,
            //    CorrectAnswers = correctAnswersCount,
            //    TotalQuestions = examCategoryVM.Questions.Count,
            //    DateTaken = DateTime.Now,
            //};

            //_context.ExamResults.Add(examResult);
            //await _context.SaveChangesAsync();

            return View(examCategoryVM);
          }


                [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}