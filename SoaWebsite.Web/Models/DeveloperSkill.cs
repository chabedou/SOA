namespace SoaWebsite.Web.Models
{
    public class DeveloperSkill
    {
        public int DeveloperId { get; set; }
        public Developer Developer { get; set; }
        public int SkillId { get; set; }
        public Skill Skill { get; set; }
    }
}