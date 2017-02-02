using System;
using System.Collections.Generic;
using System.Linq;
using SoaWebsite.Common.Contracts;
using SoaWebsite.Services.Models;

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
                if (skills != null && skills.Length != 0)
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
            Func<DeveloperSkill, bool> condition = s => s.Skill().Name.Contains(bySkill);
            return s => s.GetDeveloperSkills().Count() == 0 || s.GetDeveloperSkills().Where(condition).Count() > 0;
        }

        private static Func<Developer, bool> ConditionName(string byName)
        {
            Func<string, bool> condition = s => s.Contains(byName);
            return s => condition(s.LastName) || condition(s.FirstName);
        }
    }
}