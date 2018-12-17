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
    public class EmailTemplatesController: BaseApiController
    {
        public IEmailTemplateService emailTemplateService { get; set; }

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
            var hotel = this.HotelInformation;
            var rateTypeList = rateTypeService.TestVue_GetRateTypeListByHotelCode(this.HotelInformation.Hotel_Code);
            return rateTypeList.ToArray();
        }

        // GET api/values/5
        public string Get(int id)
        {
            var hotelList = userAccountService.GetUserMappedHotels("test@cendyn.com");
            var hotel = hotelService.GetHotelByDomain("http://" + "AABB.qaeupgrade.cendyn.com");

            return "test vue get";
        }


        [HttpPost]
        public string Post(EmailTemplate model)
        {

            model.Hotel_Code = "ASDF";
            model.languageID = 1;
            model.ShowWeatherYN = true;
            model.ShowRequestYN = true;
            model.ConciergeName = "eUpgrade";
            model.HasMultiCancelPolicy = false;
            model.TemplateCode = "RequestConfirmEmail";
            model.TemplateName = "RequestConfirmEmail";

            model.eMailType = "RequestConfirmEmail";
            model.HtmlYN = true;

            model.ActiveYN = true;
            model.insertDate = DateTime.Now;
            model.updateDate = DateTime.Now;

            string emailtemplateAddHeader = @"<!DOCTYPE html PUBLIC "" -//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
< html xmlns = ""http://www.w3.org/1999/xhtml"" >
 

 < head >
 
     < meta http - equiv = ""Content-Type"" content = ""text/html; charset=utf-8"" />
      
          < meta http - equiv = ""X-UA-Compatible"" content = ""IE=edge"" />
           
               < meta content = ""width=device-width; initial-scale=1.0;"" name = ""viewport"" />
              
                  < title > eUpgrade - Hotel Origami </ title >
                 

                 </ head >
                 

                 < body spellcheck = ""false"" > ";
            string emailtemplateAddFooter = @"</body>

</html>";

            bool result = emailTemplateService.AddNewEmailTemplate(model);

            return result ? "success" : "failed";
            //if (model.RateTypeCode == "delete")
            //{
            //    string result = rateTypeService.UpdateActiveYNByRateTypeID(model.id, "test@cendyn.com",0);
            //    if(string.IsNullOrEmpty(result))
            //    {
            //        return "The rate type has been successfully deleted.";
            //    }
            //    else
            //    {
            //        return "The rate type failed to be deleted. Please try again latter.";
            //    }
            //}
            //else
            //{
            //    NewRateTypeSaveDTO newRateTypeData = new NewRateTypeSaveDTO();

            //    newRateTypeData.ID = model.id;
            //    newRateTypeData.RateTypeCode = model.RateTypeCode;
            //    newRateTypeData.RateTypeCodeDescription = model.RateTypeCode;
            //    newRateTypeData.HotelCode = this.HotelInformation.Hotel_Code;
            //    newRateTypeData.ActiveYN = true;

            //    bool result = rateTypeService.UpdateByRateType(newRateTypeData);
            //    if (result)
            //    {
            //        return "The rate type has been successfully updated.";
            //    }
            //    else
            //    {
            //        return "The rate type failed to be saved. Please try again latter.";
            //    }
            //}
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
