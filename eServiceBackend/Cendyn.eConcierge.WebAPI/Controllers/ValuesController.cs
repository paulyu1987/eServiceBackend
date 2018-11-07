using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cendyn.eConcierge.Service.Interface;
using System.Web.Mvc;
using Cendyn.eConcierge.EntityModel;

namespace Cendyn.eConcierge.WebApi.Controllers
{
    public class ValuesController : BaseApiController
    {
        public IHotelService hotelService { get; set; }
        public IUserAccountService userAccountService { get; set; }
        public IRateTypeService rateTypeService { get; set; }

        // GET api/values
        public IEnumerable<Hotel> Get()
        {
            var hotelList = hotelService.GetAllHotels();
            
            return hotelList.ToArray();
        }

        // GET api/values/5
        public string Get(int id)
        {
            var hotelList = userAccountService.GetUserMappedHotels("test@cendyn.com");
            hotelService = DependencyResolver.Current.GetService(typeof(IHotelService)) as IHotelService;
            var hotel = hotelService.GetHotelByDomain("http://" + "AABB.qaeupgrade.cendyn.com");

            return "test vue get";
        }

        // POST api/values
        [System.Web.Http.AllowAnonymous]
        public string Post([FromBody]string value)
        {
            var rateTypeList = rateTypeService.GetRateTypeListByHotelCode("");
            var hotelList = userAccountService.GetUserMappedHotels("test@cendyn.com");
            var dateformat = hotelService.GetDateFormat("test");


            return "test vue post";
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]Hotel value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
