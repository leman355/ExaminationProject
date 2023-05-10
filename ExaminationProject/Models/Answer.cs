namespace ExaminationProject.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Option { get; set; }
        public bool Status { get; set; } = false;
        public bool Selected { get; set; } = false;
    }
}
