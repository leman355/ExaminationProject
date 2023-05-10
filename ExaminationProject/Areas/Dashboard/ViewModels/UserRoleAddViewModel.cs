using ExaminationProject.Models;
namespace ExaminationProject.Areas.Dashboard.ViewModels
{
    public class UserRoleAddViewModel
    {
        public User User { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
