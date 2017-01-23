using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SoaWebsite.Web.Models
{
    public class DeveloperContext : DbContext
    {
        public DeveloperContext(DbContextOptions<DeveloperContext> options)
            : base(options)
        { }

        public DbSet<Developer> Developers { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeveloperSkill>()
                .HasKey(t => new { t.DeveloperId, t.SkillId });

            modelBuilder.Entity<DeveloperSkill>()
                .HasOne(pt => pt.Developer)
                .WithMany(p => p.DeveloperSkills)
                .HasForeignKey(pt => pt.DeveloperId);

            modelBuilder.Entity<DeveloperSkill>()
                .HasOne(pt => pt.Skill)
                .WithMany(t => t.DeveloperSkills)
                .HasForeignKey(pt => pt.SkillId);
        }
    }
    
    public class Developer
    {
        [Key]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<DeveloperSkill> DeveloperSkills { get; set; }
    }

    public class Skill
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public List<DeveloperSkill> DeveloperSkills { get; set; }
    }
    public class DeveloperSkill
    {
        public int DeveloperId { get; set; }
        public Developer Developer { get; set; }

        public int SkillId { get; set; }
        public Skill Skill { get; set; }
    }
}