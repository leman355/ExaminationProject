using Microsoft.AspNetCore.Identity;

namespace ExaminationProject.Models
{
    public class User : IdentityUser
    {
        public string Surname { get; set; }
        public string? PhotoUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string Password { get; set; }
    }
}
