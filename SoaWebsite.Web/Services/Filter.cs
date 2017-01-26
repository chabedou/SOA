using System;
using SoaWebsite.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace SoaWebsite.Web.Services
{
    public class Filter
    {
        private string byName;
        private string bySkill;

        public Filter(string byName, string bySkill)
        {
            this.byName = byName == null ? "" : byName;
            this.bySkill = bySkill == null ? "" : bySkill;
        }

        public Func<Developer, bool> Condition()
        {
            Func<Developer, bool> filter = s => ConditionSkill()(s) && ConditionName()(s);
            return filter;
        }
        private Func<Developer, bool> ConditionSkill()
        {
            Func<DeveloperSkill, bool> condition = s => s.Skill.Name.Contains(bySkill);
            return s => s.DeveloperSkills.Count() == 0 || s.DeveloperSkills.Where(condition).Count() > 0;
        }

        private Func<Developer, bool> ConditionName()
        {
            Func<string, bool> condition = s => s.Contains(byName);
            return s => condition(s.LastName) || condition(s.FirstName);
        }
    }
}