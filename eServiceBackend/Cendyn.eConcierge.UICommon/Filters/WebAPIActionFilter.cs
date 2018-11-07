using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.UICommon.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Cendyn.eConcierge.UICommon.Filters
{
    public class WebAPIActionFilter : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string domainName = actionContext.Request.Headers.Authorization.Scheme;
            //string domainName = DomainHelper.GetCurrentDomain();

#if DEBUG
            //domainName = "qaeupgrade-sandylane.cendyn.com";
            //if (domainName.ToLower().Contains("localhost"))
                //domainName = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["LocalDomainName"]) ? "qaeupgrade-sandylane.cendyn.com" : ConfigurationManager.AppSettings["LocalDomainName"];
                //domainName = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["LocalDomainName"]) ? "pgaresort.managemyupgrade.com" : ConfigurationManager.AppSettings["LocalDomainName"];
                //domainName = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["LocalDomainName"]) ? "eupgrade-origami.cendyn.com" : ConfigurationManager.AppSettings["LocalDomainName"];
                //domainName = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["LocalDomainName"]) ? "RVMELB.qaeupgrade.cendyn.com" : ConfigurationManager.AppSettings["LocalDomainName"];
            //domainName = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["LocalDomainName"]) ? "hoteldel.managemyupgrade.cendyn.com" : ConfigurationManager.AppSettings["LocalDomainName"];
#endif
            var domaininfo = DependencyResolver.Current.GetService(typeof(DomainInformationModel)) as DomainInformationModel;
            domaininfo.DomainName = domainName;
            domaininfo.IsSecure = true;
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
