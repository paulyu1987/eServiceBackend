using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class HotelCurrencyItem
    {
        public int HotelCurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public string Hotel_Code { get; set; }
        public decimal USD_to_Rate { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencySymbolPosition { get; set; }
        public string CurrencyCultureID { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
