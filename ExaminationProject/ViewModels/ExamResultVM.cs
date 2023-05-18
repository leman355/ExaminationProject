using ExaminationProject.Models;

namespace ExaminationProject.ViewModels
{
    public class ExamResultVM
    {
        public List<ExamResult> ExamResults { get; set; }
        public int SelectedCategoryId { get; set; }
        public string SelectedCategoryName { get; set; }
        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }
        public List<QuestionAnswer> QuestionAnswers { get; set; }
        public List<int> SelectedAnswerIds { get; set; }
        public List<int> CorrectAnswerIds { get; set; }
        public int CorrectAnswerCount { get; set; }
    }
}