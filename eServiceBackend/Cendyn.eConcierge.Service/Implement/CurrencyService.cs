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
    public class CurrencyService : ServiceBase, ICurrencyService
    {
        public IL_CurrencyRepository lCurrencyRepo { get; set; }
        public IHotels_CurrencyRepository hotelsCurrencyRepo { get; set; }
        public IPlannerEventRepository plannerEventRepo { get; set; }

        public string GetCurrencySymbol (string currencyCode)
        {
            return lCurrencyRepo.GetAll().Where(x => x.Code == currencyCode).Select(x => x.EncodedSymbol).FirstOrDefault();
        }

        public List<ListItemDTO> GetHotelActiveCurrency(string hotelCode)
        {
            List<ListItemDTO> currency = new List<ListItemDTO>();
            try
            {
                currency = (from hc in hotelsCurrencyRepo.GetAll()
                            join lc in lCurrencyRepo.GetAll() on hc.Code equals lc.Code
                            where hc.Hotel_Code == hotelCode && hc.Active == true
                            select new ListItemDTO()
                            {
                                DisplayName = lc.Code + " - " + lc.EncodedSymbol,
                                Value = lc.Code
                            }
                            ).ToList();
            }
            catch (Exception ex)
            {
                var message = ex.ToString();
            }
            return currency;
        }

        public List<PriceItem> GetHotelActiveCurrency(string hotelCode, decimal? price)
        {
            List<PriceItem> list = new List<PriceItem>();
            PriceItem item = null;
            IList<HotelCurrencyItem> currencies = plannerEventRepo.GetHotelActiveCurrency(hotelCode);
            if (currencies == null || !currencies.Any() || !price.HasValue)
            {
                item = new PriceItem {
                    CurrencyCode = "USD",
                    CurrencyCultureID = "en-US",
                    CurrencySymbol = "$",
                    CurrencySymbolPosition = "Left",
                    Price = price
                };
                list.Add(item);
            }
            else
            {
                foreach (var c in currencies)
                {
                    item = new PriceItem
                    {
                        CurrencyCode = c.CurrencyCode,
                        CurrencyCultureID = c.CurrencyCultureID,
                        CurrencySymbol = c.CurrencySymbol,
                        CurrencySymbolPosition = c.CurrencySymbolPosition,
                        Price = price * c.USD_to_Rate
                    };
                    list.Add(item);
                }
            }
            return list;
        }
    }
}
