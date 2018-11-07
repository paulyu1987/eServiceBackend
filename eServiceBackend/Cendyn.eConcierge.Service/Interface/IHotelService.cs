using Cendyn.eConcierge.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface IHotelService
    {
        Hotel GetHotelByCode(string hotelCode);
        Hotel GetHotelByDomain(string domainName);
        int GetTemplateNumByHotelCode(string hotelcode);
        int GetLangIdByHCandCurLangId(string hotelCode, string curLangId, bool hasMultilanguage = false);
        Hotel GetHotelNameByUser(string logConfirmationNum);
        string GetHotelNameByCode(string hotelCode);
        string GetDateFormat(string hotelCode);
        List<string> Get2WaysHotels();
        string GetBrandCss(string hotelCode, string brandCode);
        string GetCurrencySymbol(string hotelCode);
        List<Hotel> GetAllHotels();
    }
}
