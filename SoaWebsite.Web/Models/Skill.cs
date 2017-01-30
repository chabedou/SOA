using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SoaWebsite.Web.Models
{
    public class Skill
    {
        [Key]
        public int ID { get; set; }
        [RegularExpression("^([a-zA-Z0-9 .&#+'-]+)$")]
        [Required]
        public string Name { get; set; }
        public List<DeveloperSkill> DeveloperSkills { get; set; }

    }
}