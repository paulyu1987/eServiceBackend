using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Cendyn.eConcierge.EntityModel
{
    public class eUpgradeOptionDTO
    {
        public Nullable<int> ID { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string Image { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyCultureID { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string CancellationTerm { get; set; }
        public string RoomLongDescription { get; set; }
        public int? DescOrder { get; set; }
        public string PriceDesc { get; set; }
        public string AddOnYN { get; set; }
        public string ImageYN { get; set; }
        public Nullable<short> Threshold { get; set; }
        public string RequestCheckboxLabel { get; set; }
        public IList<RoomImage> RoomTypeImages { get; set; }
        public IList<PriceItem> OptionPrices { get; set; }
    }

    public class PriceItem
    {
        public int ItemID { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencySymbolPosition { get; set; }
        public string CurrencyCultureID { get; set; }
        public decimal? Price { get; set; }
        public string Price_Display
        {
            get
            {
                return Price.HasValue ?
                    Price.Value.ToString("###,##0.00", CultureInfo.GetCultureInfo(string.IsNullOrWhiteSpace(CurrencyCultureID) ? "en-US" : CurrencyCultureID)) :
                    string.Empty;
            }
        }

        public PriceItem ()
        {
            CurrencyCode = "USD";
            CurrencySymbol = "$";
            CurrencySymbolPosition = "right";
            CurrencyCultureID = "en-US";
            Price = null;
        }
    }
}
