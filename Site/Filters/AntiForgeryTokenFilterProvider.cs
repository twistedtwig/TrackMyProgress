using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Site.Filters
{
    //This will add ValidateAntiForgeryToken Attribute to all HttpPost action methods
    public class AntiForgeryTokenFilterProvider : IFilterProvider
    {
        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var result = new List<Filter>();

            string incomingVerb = controllerContext.HttpContext.Request.HttpMethod;

            if (String.Equals(incomingVerb, "POST", StringComparison.OrdinalIgnoreCase))
            {
                result.Add(new Filter(new ValidateAntiForgeryTokenAttribute(), FilterScope.Global, null));
            }

            return result;
        }
    }
}