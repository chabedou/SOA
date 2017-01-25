using System;
using SoaWebsite.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace SoaWebsite.Web.Controllers
{
    public class DeveloperSorter
    {
        private string order;
        private string field;

        public DeveloperSorter(string fieldWithReverse){
            var parameters=fieldWithReverse.Split('.');
            field=parameters[0];
            order= parameters.Length==1? "asc" : parameters[1];
        }
        private static Func<Developer,Object> Field(string field){
            if(field=="FirstName")
            {
                return item=>item.FirstName;
            }
            else
            {
                return item=>item.LastName;
            }
        }
        public  IOrderedEnumerable<Developer> Sort(IEnumerable<Developer> developers)
        {
            Func<Developer,Object> property=Field(field);
            if(order=="asc")
            {
                return developers.OrderBy(property);
            }
            else if(order=="desc")
            {
                return developers.OrderByDescending(property);
            }
            else
            {
                throw new Exception("unsupported");
            }
        }
        
    }
}