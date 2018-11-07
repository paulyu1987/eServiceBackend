using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public partial class AvailableRoomDTO
    {
        public int ID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string RoomType { get; set; }
        public string RoomTypeCodeUpgrade { get; set; }
      //  public Nullable<System.DateTime> ArrivalDate { get; set; }
      //  public Nullable<System.DateTime> DepartureDate { get; set; }
        public string ArrivalDate { get; set; }
        public string DepartureDate { get; set; }
        public string NightsOfStay { get; set; }
        public Nullable<decimal> USDPrice { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string HotelCode { get; set; }
        public string LoginConfirmationNum { get; set; }
        public string RateType { get; set; }
        
    }
}
