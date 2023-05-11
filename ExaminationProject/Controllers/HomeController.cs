using ExaminationProject.Areas.Dashboard.ViewModels;
using ExaminationProject.Data;
using ExaminationProject.Models;
using ExaminationProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
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

            var viewModel = new ExamCategoryVM
            {
                SelectedCategoryId = id,
                SelectedCategoryName = examCategory.CategoryName,
                Questions = questions,
                Answers = answers,
                QuestionAnswers = questionAnswers,
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

            var viewModel = new ExamCategoryVM
            {
                SelectedCategoryId = id,
                SelectedCategoryName = examCategory.CategoryName,
                Questions = questions,
                Answers = answers,
                QuestionAnswers = questionAnswers,
                SelectedAnswerIds = selectedAnswerIds,
            };
              return RedirectToAction(nameof(Result));
            //return View(viewModel);
        }
























        //[HttpPost("examcategory/{id}")]
        //public IActionResult ExamCategory(int id, int[] selectedAnswerIds)
        //{
        //    var examCategory = _context.ExamCategories.Where(x => x.Id == id).FirstOrDefault();
        //    var questions = _context.Questions.Where(x => x.ExamCategoryId == id).ToList();
        //    var questionIds = questions.Select(x => x.Id).ToList();
        //    var questionAnswers = _context.QuestionAnswers
        //        .Include(x => x.Answer)
        //        .Where(x => questionIds.Contains(x.QuestionId))
        //        .ToList();
        //    var answers = questionAnswers.Select(x => x.Answer).Distinct().ToList();


        //    // Проверяем, есть ли выбранные ответы для каждого вопроса


        //    var viewModel = new ExamCategoryVM
        //    {
        //        SelectedCategoryId = id,
        //        SelectedCategoryName = examCategory.CategoryName,
        //        Questions = questions,
        //        Answers = answers,
        //        QuestionAnswers = questionAnswers,
        //        SelectedAnswerIds = selectedAnswerIds,
        //    };

            //    foreach (var questionAnswer in viewModel.QuestionAnswers)
            //    {
            //        if (questionAnswer.Answer.Status == true)
            //        {
            //            // Ищем выбранный ответ для этого вопроса
            //            var selectedAnswer = selectedAnswers.FirstOrDefault(x => x == questionAnswer.Answer.Id);
            //            if (selectedAnswer != 0)
            //            {
            //                // Устанавливаем свойство selected в true для правильного ответа
            //                questionAnswer.Answer.Selected = true;
            //            }
            //        }
            //    }



            //    // Если есть ошибки валидации, возвращаем представление с сообщениями об ошибках
            //    if (!ModelState.IsValid)
            //    {
            //        return View(viewModel);
            //    }

            //    // Подсчитываем количество правильных ответов и общее количество вопросов
            //    int correctAnswersCount = 0;
            //    int totalQuestionsCount = 0;
            //    foreach (var question in viewModel.Questions)
            //    {
            //        totalQuestionsCount++;
            //        var selectedAnswersForQuestion = selectedAnswers.Where(x => x.ToString().StartsWith(question.Id.ToString())).ToList();
            //        if (selectedAnswersForQuestion.Count == 1)
            //        {
            //            var selectedAnswerId = selectedAnswersForQuestion[0];
            //            var questionAnswer = viewModel.QuestionAnswers.FirstOrDefault(x => x.QuestionId == question.Id && x.AnswerId == selectedAnswerId);
            //            if (questionAnswer != null && questionAnswer.Answer.Status == true)
            //            {
            //                correctAnswersCount++;
            //            }
            //        }
            //    }

            //    var examResult = new ExamResult
            //    {
            //        UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value, // идентификатор текущего пользователя
            //        ExamCategoryId = id,
            //        CorrectAnswers = correctAnswersCount,
            //        TotalQuestions = totalQuestionsCount,
            //        DateTaken = DateTime.UtcNow
            //    };

            //    // Добавляем результат экзамена в базу данных
            //    _context.ExamResults.Add(examResult);
            //    _context.SaveChanges();

            //    // Если ошибок нет, перенаправляем на другую страницу
        //    return RedirectToAction(nameof(Result));
        //}

        public IActionResult Result(int id)
        {
            var results = _context.ExamResults
                .Where(x => x.ExamCategoryId == id && x.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .OrderByDescending(x => x.DateTaken)
                .ToList();

            var viewModel = new ExamResultVM
            {
                ExamCategory = _context.ExamCategories.Find(id),
                ExamResults = results
            };

            return View(viewModel);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}