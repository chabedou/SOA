using Microsoft.EntityFrameworkCore;

namespace Application.Models
{
    public class ResumesContext : DbContext
    {
        public ResumesContext(DbContextOptions<ResumesContext> options)
            : base(options)
        {
        }

        public DbSet<Developer> Users { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Skill> Resumes { get; set; }
    }

    public class Developer
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }

    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Resume
    {
        public int Id { get; set; }
        public int SkillId { get; set; }
        public int DevelopperId { get; set; }
    }
}