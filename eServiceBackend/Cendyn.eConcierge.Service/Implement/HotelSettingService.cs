using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Service.Interface;

namespace Cendyn.eConcierge.Service.Implement
{
    public class HotelSettingService : ServiceBase, IHotelSettingService
    {
        public IHotelSettingRepository hotelSettingRepo { get; set; }

        public HotelSetting GetHotelSettingMessage(string hotelCode, string key)
        {
            return GetHotelSettingMessage(hotelCode, key, "en-US");
        }

        public HotelSetting GetHotelSettingMessage(string hotelCode, string key, string cultureID)
        {
            return hotelSettingRepo.Get(p => p.Hotel_Code == hotelCode && p.SettingKey == key && p.LangId == cultureID);
        }

        public string GetHotelSettingValue(string hotelCode, string key)
        {
            return GetHotelSettingValue(hotelCode, key, "en-US");
        }

        public string GetHotelSettingValue(string hotelCode, string key, string cultureID)
        {
            var hotelsetting = GetHotelSettingMessage(hotelCode, key, cultureID);
            if (hotelsetting != null && !string.IsNullOrWhiteSpace(hotelsetting.settingValue))
                return hotelsetting.settingValue;
            else
                return string.Empty;
        }

        public List<HotelSettingDTO> GetHotelSettings(string hotelCode)
        {

            return GetHotelSettings(hotelCode, "en-US");

        }

        public List<HotelSettingDTO> GetHotelSettings(string hotelCode, string cultureID)
        {
            List<HotelSettingDTO> ret = new List<HotelSettingDTO>();
            List<HotelSettingDTO> defaultSetting = hotelSettingRepo.GetAll().Where(x => x.Hotel_Code == hotelCode && x.LangId == "en-US").ToList().AsQueryable().Select(x => new HotelSettingDTO()
            {
                LanguageID = x.languageId ?? 1,
                CultureID = x.LangId,
                SettingKey = x.SettingKey,
                SettingValue = x.settingValue
            }).ToList();
            if (cultureID.Equals("en-US"))
            {
                ret = defaultSetting;
            }
            else
            {
                List<HotelSettingDTO> cultureSetting = hotelSettingRepo.GetAll().Where(x => x.Hotel_Code == hotelCode && x.LangId == cultureID).ToList().AsQueryable().Select(x => new HotelSettingDTO()
                {
                    LanguageID = x.languageId ?? 1,
                    CultureID = x.LangId,
                    SettingKey = x.SettingKey,
                    SettingValue = x.settingValue
                }).ToList();

                if (cultureSetting != null) ret = cultureSetting;
                else ret = defaultSetting;
            }
            return ret;
        }
 
    }
}
