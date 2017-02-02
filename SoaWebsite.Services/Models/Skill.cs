using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SoaWebsite.Common.Contracts;
using System;
using System.Linq;

namespace SoaWebsite.Services.Models
{
    public class Skill : ISkill
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter skill name")]
        [StringLength(50, ErrorMessage = "The skill name must be less than {1} characters.")]
        [Display(Name = "Skill name:")]
        [RegularExpression("^([a-zA-Z0-9 .&#+'-]+)$")]
        public string Name { get; set; }
        public  List<DeveloperSkill> DeveloperSkills { get; set; }

        public List<IDeveloperSkill> GetDeveloperSkills()
        {
            return DeveloperSkills.Select(x=> x as IDeveloperSkill).ToList();
        }
    }
}