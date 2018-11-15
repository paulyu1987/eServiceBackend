using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace Cendyn.eConcierge.WebApi.Helper
{
    public class ActionAuthorizeAttribute: AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //check ticket from http header
            var domainanme = actionContext.Request;
            var authorization = actionContext.Request.Headers.Authorization;
            if ((authorization != null) && (authorization.Parameter != null))
            {
                //encrypt ticket,check user
                var encryptTicket = authorization.Parameter;
                if (ValidateTicket(encryptTicket))
                {
                    base.IsAuthorized(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            //if ticket is null,return 401
            else
            {
                base.IsAuthorized(actionContext);
                //var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                //bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                //if (isAnonymous) base.OnAuthorization(actionContext);
                //else HandleUnauthorizedRequest(actionContext);
            }
        }

        //check user（database check）
        private bool ValidateTicket(string encryptTicket)
        {
            ////encryptTicket
            //var strTicket = FormsAuthentication.Decrypt(encryptTicket).UserData;
            ////username pwd
            //var index = strTicket.IndexOf("&");
            //string strUser = strTicket.Substring(0, index);
            //string strPwd = strTicket.Substring(index + 1);

            try
            {
                var credentials = Encoding.ASCII.GetString(Convert.FromBase64String(encryptTicket));
                int splitOn = credentials.IndexOf(':');

                string strUser = credentials.Substring(0, splitOn);
                string strPwd = credentials.Substring(splitOn + 1);
                if (strUser == "test" && strPwd == "123")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch { }

            return false;

        }
    }
}