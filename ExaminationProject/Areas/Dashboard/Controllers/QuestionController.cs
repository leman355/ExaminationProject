using ExaminationProject.Areas.Dashboard.ViewModels;
using ExaminationProject.Data;
using ExaminationProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Areas.Dashboard.Controllers
{
    [Area(nameof(Dashboard))]
    public class QuestionController : Controller
    {
        private readonly AppDbContext _context;

        public QuestionController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

           var questions = _context.Questions.Include(x => x.ExamCategory).ToList();


           // var questions = _context.Questions.ToList();
            var questionAnswers = _context.QuestionAnswers.Include(x => x.Answer).ToList();
            var answer = _context.Answers.ToList();

            QuestionVM vm = new()
            {
                Questions = questions,
                QuestionAnswers = questionAnswers,
                Answers = answer,
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var examCategory = _context.ExamCategories.Where(x => x.IsDeleted == false).ToList();

            if (examCategory.Count == 0)
            {
                return RedirectToAction("Create", "ExamCategory");
            }

            ViewData["ExamCategories"] = await _context.ExamCategories.ToListAsync();
            //ViewData["Answ"] = await _context.Answers.ToListAsync();          
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Question question, string[] Option, bool IsDeleted, int examCategoryId)
        {
            try
            {
            
                question.IsDeleted = IsDeleted;
                question.ExamCategoryId = examCategoryId;
                await _context.Questions.AddAsync(question);
                await _context.SaveChangesAsync();

                for (int i = 0; i < Option.Length; i++)
                {
                    if (Option[i] != null)
                    {
                        bool.TryParse(Request.Form[$"Status_{i}"], out bool status);
                        Answer answer = new()
                        {
                            Option = Option[i],
                            Status = status,
                        };
                        await _context.Answers.AddAsync(answer);
                        await _context.SaveChangesAsync();

                        QuestionAnswer questionAnswer = new()
                        {
                            QuestionId = question.Id,
                            AnswerId = answer.Id,
                        };
                        await _context.QuestionAnswers.AddAsync(questionAnswer);
                        await _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var examCategory = _context.ExamCategories.Where(x => x.IsDeleted == false).ToList();

            if (examCategory.Count == 0)
            {
                return RedirectToAction("Create", "ExamCategory");
            }
            try
            {
                var question = await _context.Questions.Include(x => x.ExamCategory).FirstOrDefaultAsync(x => x.Id == id);
                var answers = _context.Answers.ToList();
                var questionAnswers = _context.QuestionAnswers.Where(x => x.QuestionId == question.Id).ToList();
                var examCategories = _context.ExamCategories.ToList();

                QuestionEditVM editVM = new()
                {
                    Questions = question,
                    Answers = answers,
                    QuestionAnswers = questionAnswers,
                    ExamCategories = examCategories,
                };

                return View(editVM);
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Question question, string[] Option, bool IsDeleted, int examCategoryId)
        {
            try
            {
                question.IsDeleted = IsDeleted;
                question.ExamCategoryId = examCategoryId;
                _context.Questions.Update(question);
                await _context.SaveChangesAsync();

                var questionAnswers = _context.QuestionAnswers.Where(x => x.QuestionId == question.Id).ToList();
                foreach (var qa in questionAnswers)
                {
                    var answer = _context.Answers.FirstOrDefault(a => a.Id == qa.AnswerId);
                    if (answer != null)
                    {
                        _context.Answers.Remove(answer);
                    }
                }
                _context.QuestionAnswers.RemoveRange(questionAnswers);
                await _context.SaveChangesAsync();

            
                for (int i = 0; i < Option.Length; i++)
                {
                    if (Option[i] != null)
                    {
                        bool.TryParse(Request.Form[$"Status_{i}"], out bool status);
                        Answer answer = new()
                        {
                            Option = Option[i],
                            Status = status,
                        };
                        await _context.Answers.AddAsync(answer);
                        await _context.SaveChangesAsync();

                        QuestionAnswer questionAnswer = new()
                        {
                            QuestionId = question.Id,
                            AnswerId = answer.Id
                        };
                        await _context.QuestionAnswers.AddAsync(questionAnswer);
                        await _context.SaveChangesAsync();
                    }
                }


                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var question = await _context.Questions.SingleOrDefaultAsync(x => x.Id == id);
            return View(question);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Question question)
        {
            try
            {
                var qt = await _context.Questions.SingleOrDefaultAsync(x => x.Id == question.Id);
                qt.IsDeleted = true;
                _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}