using NPOI.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.ExcelModel
{
    public class RateTypeExportExcelModel
    {
        [Column(Index = 0, Title = "Hotel Code")]
        public string HotelCode { get; set; }

        [Column(Index = 1, Title = "Rate Type Code")]
        public string RateTypeCode { get; set; }

        [Column(Index = 2, Title = "Rate Type Code Description")]
        public string RateTypeCodeDescription { get; set; }

        [Column(Index = 3, Title = "Insert Date")]
        public string InsertDate { get; set; }

        [Column(Index = 4, Title = "Update Date")]
        public string UpdateDate { get; set; }

        [Column(Index = 5, Title = "Active?")]
        public bool IsActive { get; set; }
    }
}
