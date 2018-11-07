using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.EntityModel;
using RepositoryT.Infrastructure;

namespace Cendyn.eConcierge.Data.Repository.eConcierge.Interface
{
    public partial interface IHotels_LanguagesRepository : IRepository<Hotels_Languages>
    {
        IList<Hotels_Languages> GetHotelLanguages(string hotelCode);
        List<ListItemDTO> GetHotelActiveLanguages(string hotelCode);
        HotelLanguageItem GetHotelLanguage(string hotelCode, string cultureID);
    }
}
