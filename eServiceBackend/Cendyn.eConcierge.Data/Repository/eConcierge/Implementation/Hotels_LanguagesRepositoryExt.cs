using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.Core.EntityFrameworkRepository;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;

namespace Cendyn.eConcierge.Data.Repository.eConcierge.Implementation
{
    public partial class Hotels_LanguagesRepository : EntityRepository<Hotels_Languages, ConciergeEntities>, IHotels_LanguagesRepository
    {
        public IList<Hotels_Languages> GetHotelLanguages(string hotelCode)
        {
            IList<Hotels_Languages> languages;

            try
            {
                var query = (from g in this.DataContext.Hotels_Languages
                             join l in this.DataContext.Languages on g.LanguageID equals l.LanguageID
                             where l.Active == true && g.Active == true && g.Hotel_Code == hotelCode
                             select g);

                languages = query.ToList();
                return languages;
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                return null;
            }
        }

        public List<ListItemDTO> GetHotelActiveLanguages(string hotelCode)
        {
            List<ListItemDTO> langs = new List<ListItemDTO>();
            try
            {
                var query = (from hl in this.DataContext.Hotels_Languages
                             join l in this.DataContext.Languages on hl.LanguageID equals l.LanguageID
                             where l.Active == true && hl.Active == true && hl.Hotel_Code == hotelCode
                             select new ListItemDTO
                             {
                                 DisplayName = l.DicLanguageName,
                                 Value = l.DicLanguageId
                             });
                langs = query.ToList();
            }
            catch (Exception ex)
            {
                var message = ex.ToString();
                langs = new List<ListItemDTO>()
                {
                    new ListItemDTO()
                    {
                        DisplayName = "English",
                        Value = "en-US"
                    }
                };
            }
            return langs;
        }

        public HotelLanguageItem GetHotelLanguage(string hotelCode, string cultureID)
        {
            HotelLanguageItem languages = new HotelLanguageItem(hotelCode);

            try
            {
                var query = (from g in this.DataContext.Hotels_Languages
                             join hc in this.DataContext.Hotels_Currency on g.CurrencyCode equals hc.Code
                             join l in this.DataContext.Languages on g.LanguageID equals l.LanguageID
                             join c in this.DataContext.L_Currency on g.CurrencyCode equals c.Code
                             where l.Active == true 
                             && g.Active == true && g.Hotel_Code == hotelCode && g.CultureID == cultureID
                             && hc.Active == true && hc.Hotel_Code == hotelCode
                             select new HotelLanguageItem () {
                                ID = g.ID,
                                LanguageID = g.LanguageID,
                                Hotel_Code = g.Hotel_Code,
                                Active = g.Active,
                                CultureID = g.CultureID,
                                DateFormat = g.DateFormat,
                                CurrencyCode = g.CurrencyCode,
                                CurrencySymbol = c.Symbol
                             });
                languages = query.FirstOrDefault();                
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                languages = null;
            }
            if (languages == null)
            {
                languages = new HotelLanguageItem(hotelCode);
            }
            return languages;
        }

    }
}
