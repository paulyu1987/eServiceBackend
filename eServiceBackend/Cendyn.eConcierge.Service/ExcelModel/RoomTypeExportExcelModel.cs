using NPOI.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.ExcelModel
{
    public class RoomTypeExportExcelModel
    {
        [Column(Index = 0, Title = "Hotel Code")]
        public string Hotel_Code { get; set; }

        [Column(Index = 1, Title = "Code")]
        public string RoomCode { get; set; }

        [Column(Index = 2, Title = "Name")]
        public string RoomDescription { get; set; }
        [Column(Index = 3, Title = "Upgrade Type")]
        public string UpgradeType { get; set; }

        [Column(Index = 4, Title = "Display Image")]
        public string ImageYN { get; set; }

        [Column(Index = 5, Title = "Price Description")]
        public string PriceDesc { get; set; }

        [Column(Index = 6, Title = "Enable?")]
        public bool IsActive { get; set; }

        public string PerNightCharge { get; set; }

        public string UpgradeTypeDisplayName { get; set; }
    }
}
