using ExaminationProject.Models;

namespace ExaminationProject.Areas.Dashboard.ViewModels
{
    public class QuestionVM
    {
        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }
        public List<QuestionAnswer> QuestionAnswers { get; set; }
    }
}
