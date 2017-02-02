using System;
using SoaWebsite.Common.Contracts;
using System;
using System.Collections.Generic;

namespace SoaWebsite.Web.ViewModels
{
    public class MainViewModel
    {
        public List<string> OneOfSkills { get; set; }
        public List<string> AllSkills { get; set; }
        public IEnumerable<IDeveloper> Developers { get; set; }
        public IDeveloper SelectedForDetails { get; set; }
        public IDeveloper SelectedForEdit { get; set; }
    }
}