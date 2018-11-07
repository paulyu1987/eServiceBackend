using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Cendyn.eConcierge.WebApi.Helper;
using Cendyn.eConcierge.UICommon.Filters;

namespace Cendyn.eConcierge.WebApi.Controllers
{
    [WebAPIActionFilter]
    [ActionAuthorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BaseApiController : ApiController
    {
    }
}