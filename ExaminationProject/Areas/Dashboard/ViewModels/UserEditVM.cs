using ExaminationProject.Models;

namespace ExaminationProject.Areas.Dashboard.ViewModels
{
    public class UserEditVM
    {
        public User User { get; set; }
        public List<Group> Groups { get; set; }
        public List<UserGroup> UserGroups { get; set; } 
    }
}
