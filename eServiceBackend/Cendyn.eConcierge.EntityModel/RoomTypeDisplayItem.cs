using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class RoomTypeDisplayItem
    {
        public int RoomTypeCodeID { get; set; }
        public string RoomDescription { get; set; }
        public string RoomLongDescription { get; set; }
        public string PriceDesc { get; set; }
        public string CultureID { get; set; }
        public int LanguageID { get; set; }
    }
}
