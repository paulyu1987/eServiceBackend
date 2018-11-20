using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Cendyn.eConcierge.WebApi.Helper;
using Cendyn.eConcierge.WebApi.Filters;
using Cendyn.eConcierge.EntityModel;

namespace Cendyn.eConcierge.WebApi.Controllers
{
    [HotelInformationFilter]
    [ActionAuthorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BaseApiController : ApiController
    {
        public Hotel HotelInformation { get; set; }
    }
}