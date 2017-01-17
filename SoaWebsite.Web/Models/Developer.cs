using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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
            modelBuilder.Entity<Skill>()
                .HasOne(p => p.Developer)
                .WithMany(b => b.Skills)
                .HasForeignKey(p => p.DeveloperID)
                .HasConstraintName("ForeignKey_Skill_Developer");
        }
    }

    public class Developer
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Skill> Skills { get; set; }
    }

    public class Skill
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int DeveloperID { get; set; }
        public Developer Developer { get; set; }
    }
}