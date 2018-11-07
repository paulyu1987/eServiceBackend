using Cendyn.eConcierge.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface ICurrencyService
    {
        string GetCurrencySymbol(string currencyCode);
        List<ListItemDTO> GetHotelActiveCurrency(string hotelCode);
        List<PriceItem> GetHotelActiveCurrency(string hotelCode, decimal? price);
    }
}
