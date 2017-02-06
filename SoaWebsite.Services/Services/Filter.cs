using System;
using System.Linq;
using SoaWebsite.Common.Models;

namespace SoaWebsite.Services.Services
{
    public class Filter
    {
        private string[] skills;

        public Filter(string[] skills)
        {
            this.skills = skills;
        }

        public Func<Developer, bool> Condition()
        {
            Func<Developer, bool> filter = s =>
            {
                var hasAllSkills = true;
                if (skills != null)
                {
                    foreach (var skill in skills)
                    {
                        hasAllSkills = hasAllSkills && ConditionSkill(skill)(s);
                    }
                }
                return hasAllSkills;
            };
            return filter;
        }
        private static Func<Developer, bool> ConditionSkill(string bySkill)
        {
            Func<DeveloperSkill, bool> condition = s => s.Skill.Name.Contains(bySkill);
            return s => s.DeveloperSkills.Where(condition).Count() > 0;
        }
    }
}