using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoaWebsite.Web.Models
{
    public class Developer
    {
        [Key]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<DeveloperSkill> DeveloperSkills { get; set; }
    }
}