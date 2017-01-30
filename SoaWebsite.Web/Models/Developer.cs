using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoaWebsite.Web.Models
{
    public class Developer
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter the user's first name.")]
        [StringLength(50, ErrorMessage = "The First Name must be less than {1} characters.")]
        [Display(Name = "First Name:")]
        [RegularExpression("^([a-zA-Z]+)$")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter the user's last name.")]
        [StringLength(50, ErrorMessage = "The Last Name must be less than {1} characters.")]
        [Display(Name = "Last Name:")]
        [RegularExpression("^([a-zA-Z]+)$")]
        public string LastName { get; set; }
        public List<DeveloperSkill> DeveloperSkills { get; set; }
    }
}