using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.EntityModel;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface ILanguagesService
    {
        List<string> GetActiveCultureIDs();
        List<string> GetHotelCultureIDs(string hotelCode);
        HotelLanguageItem GetHotelLanguage(string hotelCode, string cultureID);
        List<ListItemDTO> GetHotelActiveLanguages(string hotelCode);
    }
}
