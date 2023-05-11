namespace ExaminationProject.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool IsDeleted { get; set; } 
        public ExamCategory ExamCategory { get; set; }
        public int ExamCategoryId { get; set; }
    }
}
