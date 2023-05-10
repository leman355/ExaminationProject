namespace ExaminationProject.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
