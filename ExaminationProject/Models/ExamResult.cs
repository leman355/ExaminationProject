namespace ExaminationProject.Models
{
    public class ExamResult
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public int ExamCategoryId { get; set; }
        public ExamCategory ExamCategory { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public DateTime DateTaken { get; set; }
    }
}
