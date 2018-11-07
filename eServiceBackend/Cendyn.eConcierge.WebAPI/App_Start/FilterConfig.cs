using Cendyn.eConcierge.UICommon.Filters;
using Cendyn.eConcierge.WebApi.Helper;
using System.Web;
using System.Web.Mvc;
using System.Web.Http.Filters;

namespace Cendyn.eConcierge.WebApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
