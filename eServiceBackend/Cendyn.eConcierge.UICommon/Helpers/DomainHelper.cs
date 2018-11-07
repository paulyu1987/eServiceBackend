using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Cendyn.eConcierge.UICommon.Helpers
{
    public static class DomainHelper
    {
        public static string GetCurrentDomain()
        {
            return HttpContext.Current.Request.Url.Host;
        }

        public static string GetCurrentUrlScheme()
        {
            return HttpContext.Current.Request.Url.Scheme;
        }
    }
}
