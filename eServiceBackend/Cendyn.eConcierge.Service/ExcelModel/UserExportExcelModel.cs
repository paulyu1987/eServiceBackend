using NPOI.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.ExcelModel
{
    public class UserExportExcelModel
    {
        [Column(Index = 0, Title = "Hotel Code")]
        public string HotelCode { get; set; }

        [Column(Index = 1, Title = "Concierge ID")]
        public string ConciergeID { get; set; }

        [Column(Index = 2, Title = "First Name")]
        public string FName { get; set; }

        [Column(Index = 3, Title = "Last Name")]
        public string LName { get; set; }

        [Column(Index = 4, Title = "Phone")]
        public string Phone { get; set; }

        [Column(Index = 5, Title = "Active?")]
        public bool IsActive { get; set; }
    }
}
