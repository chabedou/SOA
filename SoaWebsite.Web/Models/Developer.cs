using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoaWebsite.Web.Models
{
    public class DeveloperContext : DbContext
    {
        public DeveloperContext(DbContextOptions<DeveloperContext> options)
            : base(options)
        { }

        public DbSet<Developer> Developers { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
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
    }

    public class Relationship
    {
        public int ID { get; set; }
        [ForeignKey("Developer")]
        public int DeveloperID { get; set; }
        public Developer Developer { get; set; }
        [ForeignKey("Skill")]
        public int SkillID { get; set; }
        public Skill Skill { get; set; }
    }
}