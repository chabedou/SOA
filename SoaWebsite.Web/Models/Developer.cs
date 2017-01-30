using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoaWebsite.Web.Models
{
    public class Developer
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [RegularExpression("^([a-zA-Z]+)$")]
        public string FirstName { get; set; }
        [RegularExpression("^([a-zA-Z]+)$")]
        [Required]
        public string LastName { get; set; }
        public List<DeveloperSkill> DeveloperSkills { get; set; }
    }
}