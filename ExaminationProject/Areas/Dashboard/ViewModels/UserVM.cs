using ExaminationProject.Models;

namespace ExaminationProject.Areas.Dashboard.ViewModels
{
    public class UserVM
    {
        public List<User> Users { get; set; }
        public List<Group> Groups { get; set; }
        public List<UserGroup> UserGroups { get; set; }
    }
}
