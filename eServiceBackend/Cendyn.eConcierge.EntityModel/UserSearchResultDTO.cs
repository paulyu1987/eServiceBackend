using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class UserSearchResultDTO
    {
        public int id { get; set; }
        public string HotelCode { get; set; }
        public string ConciergeID { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Phone { get; set; }
        public bool? IsActive { get; set; }
        public int? AccessLevel { get; set; }
        public string DateFormat { get; set; }
    }
}
