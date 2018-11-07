using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class EUpgradeRequestDTO
    {
        public int ID { get; set; }
        public string loginConfirmationNum { get; set; }
        public string HotelCode { get; set; }
        public string BookedRoomType { get; set; }
        public string UpgradeRoomType { get; set; }
        public Nullable<decimal> UpgradeCost { get; set; }
        public string UpgradeStatus { get; set; }
        public string DecisionPerson { get; set; }
        public Nullable<System.DateTime> OXIDateSent { get; set; }
        public string OXIMessageID { get; set; }
        public Nullable<bool> OXIDirty { get; set; }
        public string OXITransactioncode { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    }
}
