using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class HotelDTO
    {
        public string Hotel_Code { get; set; }

        public string Hotel_Name { get; set; }

        public string Hotel_Address { get; set; }

        public string Hotel_City { get; set; }

        public string Hotel_State { get; set; }

        public string Hotel_Country { get; set; }

        public string Hotel_Zip { get; set; }

        public string Hotel_Fax { get; set; }

        public string Hotel_Phone { get; set; }

        public string Hotel_WebSite { get; set; }

        public string Call800 { get; set; }

        public string BrandCode { get; set; }

        public string ChainCode { get; set; }

        public string DateFormat { get; set; }

        public bool AutoRequestProcessYN { get; set; }

        public bool CentralResNumYN { get; set; }

        public int LanguageID { get; set; }
        public string CultureID { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }

    }
}
