using System;
using SoaWebsite.Web.Models;
using System;
using System.Collections.Generic;

namespace SoaWebsite.Web.ViewModels
{
    public class DevelopersSearch
    {
        private IEnumerable<DeveloperViewModel> developers;
        private IEnumerable<string> skills;
        private string sortOrder;
    }
}