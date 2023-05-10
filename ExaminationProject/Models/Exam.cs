namespace ExaminationProject.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int ExamCategoryId { get; set; }
        public ExamCategory ExamCategory { get; set; }
    }
}
