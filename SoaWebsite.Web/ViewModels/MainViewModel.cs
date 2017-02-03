using SoaWebsite.Common.Models;
using System.Collections.Generic;

namespace SoaWebsite.Web.ViewModels
{
    public class MainViewModel
    {
        public List<string> OneOfSkills { get; set; }
        public List<string> AllSkills { get; set; }
        public IEnumerable<Developer> Developers { get; set; }
        public Developer SelectedForDetails { get; set; }
        public Developer SelectedForEdit { get; set; }
        public Developer SelectedForCreate { get; set; }

    }
}