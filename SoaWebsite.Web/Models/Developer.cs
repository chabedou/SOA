using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoaWebsite.Web.Models
{
    public class Developer
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter the user's first name.")]
        [StringLength(50,MinimumLength=3, ErrorMessage = "The First Name must have at least {2} characters and be less than {1} characters.")]
        [Display(Name = "First Name:")]
        [RegularExpression("^([a-zA-Z]+[,.]?[ ]?|[a-zA-Z]+['-]?)+$", ErrorMessage = "Invalid first name, only alpha characters, \"-\", \",\", and \".\" are allowed")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter the user's last name.")]
        [StringLength(50,MinimumLength=3, ErrorMessage = "The Last Name must have at least {2} characters and be less than {1} characters.")]
        [Display(Name = "Last Name:")]
        [RegularExpression("^([a-zA-Z]+[,.]?[ ]?|[a-zA-Z]+['-]?)+$", ErrorMessage = "Invalid last name, only alpha characters, \"-\", \",\", and \".\" are allowed")]
        public string LastName { get; set; }
        public List<DeveloperSkill> DeveloperSkills { get; set; }
    }
}