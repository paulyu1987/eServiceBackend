using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.EntityModel;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface IHotelSettingService
    {
        HotelSetting GetHotelSettingMessage(string hotelCode, string key);
        HotelSetting GetHotelSettingMessage(string hotelCode, string key, string cultureID);
        string GetHotelSettingValue(string hotelCode, string key);
        string GetHotelSettingValue(string hotelCode, string key, string cultureID);
        List<HotelSettingDTO> GetHotelSettings(string hotelCode);
        List<HotelSettingDTO> GetHotelSettings(string hotelCode, string cultureID);
    }
}
