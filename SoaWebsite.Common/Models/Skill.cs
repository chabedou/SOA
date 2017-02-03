using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoaWebsite.Common.Models
{
    public class Skill
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter skill ")]
        [StringLength(50, ErrorMessage = "skill must have less than {1} characters.")]
        [Display(Name = "Skill name:")]
        [RegularExpression("^([a-zA-Z0-9 .&#+'-]+)$", ErrorMessage = "Invalid skill")]
        public string Name { get; set; }

        public  List<DeveloperSkill> DeveloperSkills { get; set; }
    }
}