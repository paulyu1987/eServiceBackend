using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class RequestSearchCriteriaDTO
    {
        public string UserName { get; set; }
        public string HotelName { get; set; }
        public DateTime? DateRequested { get; set; }

        public DateTime? ArrivalDateFrom { get; set; }

        public DateTime? ArrivalDateTo { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string BookedRoomType { get; set; }

        public string UpgraedRoomType { get; set; }

        public string RequestStatus { get; set; }

        public string Email { get; set; }

    }
}
