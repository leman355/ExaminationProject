namespace ExaminationProject.Models
{
    public class QuestionAnswer
    {
        public int Id { get; set; }
        public Answer Answer { get; set; }
        public int AnswerId { get; set; }
        public Question Question { get; set; }
        public int QuestionId { get; set; }
    }
}
