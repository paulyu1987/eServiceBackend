using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class HotelLanguageItem
    {
        public int ID { get; set; }
        public string Hotel_Code { get; set; }
        public int? LanguageID { get; set; }
        public bool? Active { get; set; }
        public string CultureID { get; set; }
        public string DateFormat { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public string LanguageName { get; set; }

        public HotelLanguageItem(string hotelCode)
        {
            ID = 0;
            Hotel_Code = hotelCode;
            LanguageID = 1;
            Active = true;
            CultureID = "en-US";
            DateFormat = "MM/DD/YYYY";
            CurrencyCode = "USD";
            CurrencySymbol = "$";
            LanguageName = "English";
        }

        public HotelLanguageItem()
        {
            ID = 0;
            Hotel_Code = "";
            LanguageID = 1;
            Active = true;
            CultureID = "en-US";
            DateFormat = "MM/DD/YYYY";
            CurrencyCode = "USD";
            CurrencySymbol = "$";
            LanguageName = "English";
        }
    }
}
