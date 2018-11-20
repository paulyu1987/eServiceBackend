using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Service.Interface;
using Cendyn.eConcierge.WebApi.Controllers;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Cendyn.eConcierge.WebApi.Filters
{
    public class HotelInformationFilter : System.Web.Http.Filters.ActionFilterAttribute
    {
        IHotelService hotelService;
        private BaseApiController controller;
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string domainName = actionContext.Request.Headers.Referrer.Authority;

            if (domainName.ToLower().Contains("localhost"))
            {
                domainName = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["LocalDomainName"]) ? "RVMELB.qaeupgrade.cendyn.com" : ConfigurationManager.AppSettings["LocalDomainName"];
            }
            
            var domaininfo = DependencyResolver.Current.GetService(typeof(DomainInformationModel)) as DomainInformationModel;
            domaininfo.DomainName = domainName;
            domaininfo.IsSecure = true;

            hotelService = DependencyResolver.Current.GetService(typeof(IHotelService)) as IHotelService;
            controller = actionContext.ControllerContext.Controller as BaseApiController;
            controller.HotelInformation = hotelService.GetHotelByDomain("http://" + domainName);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var objectContent = actionExecutedContext.Response.Content as ObjectContent;
            if (objectContent != null)
            {
                var type = objectContent.ObjectType; //type of the returned object
                var value = objectContent.Value; //holding the returned value
            }

            Debug.WriteLine("ACTION 1 DEBUG  OnActionExecuted Response " + actionExecutedContext.Response.StatusCode.ToString());
        }
    }
}
