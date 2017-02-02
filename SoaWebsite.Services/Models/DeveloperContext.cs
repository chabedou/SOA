using Microsoft.EntityFrameworkCore;
using SoaWebsite.Common.Contracts;

namespace SoaWebsite.Services.Models
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
}