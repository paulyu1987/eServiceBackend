using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cendyn.eConcierge.Service.Implement
{
    public class LanguagesService : ServiceBase, ILanguagesService
    {
        public ILanguageRepository langRepo { get; set; }
        public IHotels_LanguagesRepository hotelLangRepo { get; set; }

        public List<string> GetActiveCultureIDs()
        {
            return langRepo.GetAll().Where(x => x.Active == true).Select(x => x.DicLanguageId).ToList();
        }

        public List<string> GetHotelCultureIDs(string hotelCode)
        {
            var langs = hotelLangRepo.GetHotelLanguages(hotelCode);
            if (langs != null && langs.Any()) { return langs.Select(x => x.CultureID).ToList(); }
            else return new List<string>();
        }
        public HotelLanguageItem GetHotelLanguage(string hotelCode, string cultureID)
        {
            return hotelLangRepo.GetHotelLanguage(hotelCode, cultureID);
        }

        public List<ListItemDTO> GetHotelActiveLanguages(string hotelCode)
        {
            List<ListItemDTO> langs = hotelLangRepo.GetHotelActiveLanguages(hotelCode);
            if (langs == null) langs = new List<ListItemDTO>();

            return langs;
        }

    }
}
