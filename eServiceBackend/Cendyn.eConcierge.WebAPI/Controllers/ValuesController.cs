using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cendyn.eConcierge.Service.Interface;
using Cendyn.eConcierge.EntityModel;
using Newtonsoft.Json;

namespace Cendyn.eConcierge.WebApi.Controllers
{
    public class ValuesController : BaseApiController
    {
        public IHotelService hotelService { get; set; }
        public IUserAccountService userAccountService { get; set; }
        public IRateTypeService rateTypeService { get; set; }
        public class NewRateTypeModel
        {
            public int id { get; set; }
            public string HotelCode { get; set; }
            public string RateTypeCode { get; set; }
            public string RateTypeCodeDescription { get; set; }
            public DateTime InsertDate { get; set; }
            public DateTime UpdateDate { get; set; }
            public bool ActiveYN { get; set; }
        }

        // GET api/values
        public IEnumerable<ListItemDTO> Get()
        {
            //var hotelList = hotelService.GetAllHotels();

            var rateTypeList = rateTypeService.TestVue_GetRateTypeListByHotelCode("AABB");
            return rateTypeList.ToArray();
        }

        // GET api/values/5
        public string Get(int id)
        {
            var hotelList = userAccountService.GetUserMappedHotels("test@cendyn.com");
            var hotel = hotelService.GetHotelByDomain("http://" + "AABB.qaeupgrade.cendyn.com");

            return "test vue get";
        }

        // POST api/values
        [System.Web.Http.AllowAnonymous]
        [HttpPost]
        public string Post(NewRateTypeModel model)
        {
            if(model.RateTypeCode == "delete")
            {
                string result = rateTypeService.UpdateActiveYNByRateTypeID(model.id, "test@cendyn.com",0);
                if(string.IsNullOrEmpty(result))
                {
                    return "The rate type has been successfully deleted.";
                }
                else
                {
                    return "The rate type failed to be deleted. Please try again latter.";
                }
            }
            else
            {
                NewRateTypeSaveDTO newRateTypeData = new NewRateTypeSaveDTO();

                newRateTypeData.ID = model.id;
                newRateTypeData.RateTypeCode = model.RateTypeCode;
                newRateTypeData.RateTypeCodeDescription = model.RateTypeCode;
                newRateTypeData.HotelCode = "AABB";
                newRateTypeData.ActiveYN = true;

                bool result = rateTypeService.UpdateByRateType(newRateTypeData);
                if (result)
                {
                    return "The rate type has been successfully updated.";
                }
                else
                {
                    return "The rate type failed to be saved. Please try again latter.";
                }
            }
            


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
