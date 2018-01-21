using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DhaliProcurement.HtmlHelpers
{
    
    public static class HtmlNullHelpers
    {
        public static MvcHtmlString DateToString(this HtmlHelper html, object obj)
        {
            if (obj == null)
            {
                return MvcHtmlString.Empty;
            }

            if (((DateTime)obj) == DateTime.MinValue)
            {
                return MvcHtmlString.Empty;
            }

            return MvcHtmlString.Create(((DateTime)obj).ToString("dd/MM/yyyy"));
        }
    }
}