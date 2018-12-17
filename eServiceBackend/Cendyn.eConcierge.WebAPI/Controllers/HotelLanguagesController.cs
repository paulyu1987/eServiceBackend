using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cendyn.eConcierge.Service.Interface;
using Cendyn.eConcierge.EntityModel;
using Newtonsoft.Json;
using System.Web.Http.Results;
using System.Web;
using System.IO;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.WebApi.Controllers
{
    public class HotelLanguagesController : BaseApiController
    {
        public IHotelService hotelService { get; set; }
        public IUserAccountService userAccountService { get; set; }
        public IRateTypeService rateTypeService { get; set; }
        public ILanguagesService languagesService { get; set; }

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
            var languageList = languagesService.GetHotelActiveLanguages(this.HotelInformation.Hotel_Code);
            return languageList.ToArray();
        }

        // GET api/values/5
        public HotelLanguageItem Get(string id)
        {
            var LanguageList = languagesService.GetHotelLanguage(this.HotelInformation.Hotel_Code, id);

            return LanguageList;
        }


        public async Task<HttpResponseMessage> Post()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            var path = HttpContext.Current.Server.MapPath("~/Userimage/");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }


                            var filePath = HttpContext.Current.Server.MapPath("~/Userimage/" + postedFile.FileName);

                            postedFile.SaveAs(filePath);

                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
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
