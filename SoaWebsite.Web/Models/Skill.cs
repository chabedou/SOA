using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SoaWebsite.Web.Models
{
    public class Skill
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public List<DeveloperSkill> DeveloperSkills { get; set; }

    }
}