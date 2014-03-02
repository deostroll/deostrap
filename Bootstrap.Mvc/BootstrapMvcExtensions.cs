using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Web.Mvc.TwitterBootstrap
{
    public static class BootstrapMvcExtensions
    {
        public static BootstrapViewFactory Bootstrap(this HtmlHelper hlpr)
        {
            return new BootstrapViewFactory();
        }
    }
}
