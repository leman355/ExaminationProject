namespace ExaminationProject.Models
{
    public class ExamResult
    {
        public int Id { get; set; }
        public Exam Exam { get; set; }
        public int ExamId { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
    }
}
