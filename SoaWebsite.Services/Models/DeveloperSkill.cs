using SoaWebsite.Common.Contracts;

namespace SoaWebsite.Services.Models
{
    public class DeveloperSkill : IDeveloperSkill
    {
        public int DeveloperId { get; set; }
        public Developer Developer { get; set; }
        public int SkillId { get; set; }
        public Skill Skill { get; set; }
    }
}