using ExaminationProject.Models;

namespace ExaminationProject.ViewModels
{
    public class ExamResultVM
    {
        public ExamCategory ExamCategory { get; set; }
        public List<ExamResult> ExamResults { get; set; }
    }
}
