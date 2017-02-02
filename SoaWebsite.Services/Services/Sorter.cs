using System;
using System.Collections.Generic;
using System.Linq;
using SoaWebsite.Common.Contracts;

namespace SoaWebsite.Services.Services
{
    public class Sorter
    {
        private string order;
        private string field;

        public Sorter(string fieldWithReverse)
        {
            var parameters = fieldWithReverse.Split('.');
            field = parameters[0];
            order = parameters.Length == 1 ? "asc" : parameters[1];
        }
        private static Func<IDeveloper, Object> Field(string field)
        {
            if (field == "FirstName")
            {
                return item => item.FirstName;
            }
            else
            {
                return item => item.LastName;
            }
        }
        public IOrderedEnumerable<IDeveloper> Sort(IEnumerable<IDeveloper> developers)
        {
            Func<IDeveloper, Object> property = Field(field);
            if (order == "asc")
            {
                return developers.OrderBy(property);
            }
            else if (order == "desc")
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