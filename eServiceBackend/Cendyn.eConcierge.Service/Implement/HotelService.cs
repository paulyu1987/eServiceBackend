using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.Implement
{
    public class HotelService : ServiceBase, IHotelService
    {
        public IHotelRepository hotelRepo { get; set; }
        public IFGuestRepository fguestRepo { get; set; }
        public IEUpgradeBrandCssMappingRepository brandCssRepo { get; set; }
        public ICurrencyConvertRepository currencyConvertRepo { get; set; }
        public Hotel GetHotelByCode(string hotelCode)
        {
            return hotelRepo.Get(p => p.Hotel_Code == hotelCode && p.Live == true);
        }

        public Hotel GetHotelByDomain(string domainName)
        {
            return hotelRepo.Get(h => h.DomainName == domainName && h.Live == true);
        }

        public int GetTemplateNumByHotelCode(string hotelcode)
        {
            int TemplateNum = 1;
            string spName = "spGetTemplateNum";
            var pList = new Dictionary<string, object>()
            {
                 {"@hotelCode",hotelcode}
            };
            TemplateNum =  hotelRepo.ExecuteStoredProcedure<int>(spName, pList).FirstOrDefault();
            return TemplateNum;
        }

        public int GetLangIdByHCandCurLangId(string hotelCode, string curLangId, bool hasMultilanguage = false)
        {
            int langID = 1;
            string spName = "eConcierge_GetLangIdByHCandCurLangId";
            var pList = new Dictionary<string, object>()
            {
                 {"@HotelCode",hotelCode},
                 {"@DicLanguageId",curLangId},
            };

            if(hasMultilanguage)
            {
                langID = hotelRepo.ExecuteStoredProcedure<int>(spName, pList).FirstOrDefault();
                
            }
            return langID;
        }

        public Hotel GetHotelNameByUser(string logConfirmationNum)
        {
            Hotel h = (from guest in fguestRepo.GetAll()
                           join hotel in hotelRepo.GetAll() on guest.HOTEL_CODE equals hotel.Hotel_Code
                           where guest.LoginConfirmationNum == logConfirmationNum
                           select hotel).FirstOrDefault();


            return h;
        }

        public string GetHotelNameByCode(string hotelCode)
        {
            return hotelRepo.GetAll()
                        .Where(p => p.Hotel_Code == hotelCode && p.Live == true)
                        .Select(p => p.Hotel_Name)
                        .ToList<string>().FirstOrDefault();
        }

        public string GetDateFormat(string hotelCode)
        {
            return hotelRepo.GetAll()
                        .Where(p => p.Hotel_Code == hotelCode && p.Live == true)
                        .Select(p => p.DateFormat)
                        .ToList<string>().FirstOrDefault();
        }

        public List<string> Get2WaysHotels()
        {
            return hotelRepo.GetAll()
                        .Where(p => p.Live == true
                            && p.CendynPropertyId != "" && p.CendynPropertyId != null
                            && p.CenResServiceInterface != "" && p.CenResServiceInterface != null
                            && p.CenResServiceUrl != "" && p.CenResServiceUrl != null
                            && p.AutoRequestProcessYN == true)
                            .Select(p => p.Hotel_Code)
                            .ToList<string>();
        }

        public string GetBrandCss(string hotelCode, string brandCode)
        {
            string brandCss = brandCssRepo.GetAll().Where(p => p.IsActive == true && p.HotelCode == hotelCode && p.BrandCode == brandCode).Select(p => p.BrandCss).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(brandCss))
            {
                brandCss = brandCssRepo.GetAll().Where(p => p.IsActive == true && p.BrandCode == brandCode && p.IsDefault == true).Select(p => p.BrandCss).FirstOrDefault();
            }
            return brandCss;
        }

        public string GetCurrencySymbol(string hotelCode)
        {

            string hotel_LocalCurrencyCode =  hotelRepo.GetAll().Where(p => p.Hotel_Code == hotelCode && p.Live == true).Select(p => p.LocalCurrencyCode).FirstOrDefault();
            string symbol = "";
            symbol = currencyConvertRepo.GetAll().Where(p => p.Code == hotel_LocalCurrencyCode && p.Hotel_Code == hotelCode).Select(p => p.Symbol).FirstOrDefault();
            return symbol;
        }

        public List<Hotel> GetAllHotels()
        {
            return hotelRepo.GetAll().ToList();
        }


    }

}
