using ExaminationProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExaminationProject.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<ExamCategory> ExamCategories { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
        }
    }
}
