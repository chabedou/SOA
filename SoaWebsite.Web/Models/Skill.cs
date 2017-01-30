using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SoaWebsite.Web.Models
{
    public class Skill
    {
        [Key]
        public int ID { get; set; }
       
        [Required(ErrorMessage = "Please enter skill name")]
        [StringLength(50, ErrorMessage = "The skill name must be less than {1} characters.")]
        [Display(Name = "Skill name:")]
        public string Name { get; set; }
        public List<DeveloperSkill> DeveloperSkills { get; set; }

    }
}