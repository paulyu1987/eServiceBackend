using Cendyn.eConcierge.EntityModel;
using DynaCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface ISettingService
    {
        [CacheableMethod(120)]
        string GetSettingValueByKey(string key);

        Language GetLanguageByCode(string langID);
        IList<ListItemDTO> GetAllLanguages();
        IList<ListItemDTO> GetAllDateFormat();
        IList<ListItemDTO> GetAllCurrency();
        IList<ListItemDTO> GetAllCurrencyByHotelCode(string hotelCode);
        IList<HotelCurrencyItem> GetAllHotelActiveCurrency(string hotelCode);
        IList<ListItemDTO> GetAllHotelLangInfo();
        bool SaveHotelsLanguages(List<Hotels_Languages> hotelLanguage);
        bool SaveHotelsCurrency(List<Hotels_Currency> hotelCurrency);
    }
}
