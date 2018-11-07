using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class ReportSearchCriteriaDTO
    {
        public string UserName { get; set; }
        public string HotelName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime? ConfirmedDate { get; set; }
        //public DateTime? OXIDateSent { get; set; }
        public DateTime? ArrivalDateFrom { get; set; }
        public DateTime? ArrivalDateTo { get; set; }

        public string TransactionCode { get; set; }
        public string ReportUpgradeStatus { get; set; }
        public string BookedRoomType { get; set; }
    }
}
