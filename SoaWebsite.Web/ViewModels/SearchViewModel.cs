using System.Collections.Generic;

namespace SoaWebsite.Web.ViewModels
{
    public class SearchViewModel
    {
        public IEnumerable<string> AllOfSkills{get;set;}
        public IEnumerable<string> OneOfSkills{get;set;}
    }
}