using System;
using SoaWebsite.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace SoaWebsite.Web.Controllers
{
    public class DeveloperFilter
    {
        private string byName;
        private string bySkill;

        public DeveloperFilter(string byName,string bySkill)
        {
            this.byName=byName==null?"":byName;
            this.bySkill=bySkill==null?"":bySkill;
        }
        
        public Func<Developer,bool> Filter(){
            Func<Developer,bool> filter= s => conditionSkill()(s) && conditionName()(s);
            return filter;
        }
        private Func<Developer,bool> conditionSkill(){
            Func<DeveloperSkill,bool> condition=s=>s.Skill.Name.Contains(bySkill);
            return s => s.DeveloperSkills.Where(condition).Count()>0;
        }

        private Func<Developer,bool> conditionName(){
            Func<string,bool> condition=s=>s.Contains(byName);
            return s => condition(s.LastName)||condition(s.FirstName);
        }
    }
}