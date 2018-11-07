using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class GrantedRequestEmailGenerateDTO
    {
        public string BookedRoomType { get; set; }
        public string UpgradedRoomType { get; set; }
        public decimal? UpgradeCost { get; set; }
        public string PriceDesc { get; set; }
        public string PerNightCharge { get; set; }
        public string CurrencySymbol { get; set; }
    }

    public class DeniedRequestEmailGenerateDTO
    {
        public string BookedRoomType { get; set; }
        public string UpgradedRoomType { get; set; }
        public decimal? UpgradeCost { get; set; }
        public string PriceDesc { get; set; }
        public string PerNightCharge { get; set; }
        public string CurrencySymbol { get; set; }
    }

    public class GlobalEmailGuestDTO
    {
        public int RequestID { get; set; }
        public string HotelCode { get; set; }
        public string Email { get; set; }
        public string GuestName { get; set; }
    }
}
