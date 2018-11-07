using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class NewRoomTypeSaveDTO
    {
        public int id { get; set; }
        public string Hotel_Code { get; set; }
        public string RoomCode { get; set; }
        public string RoomDescription { get; set; }
        public string RoomLongDescription { get; set; }
        //public bool PriceDesc { get; set; }
        public bool PerNightCharge { get; set; }
        public string UserName { get; set; }
        public bool ImageYN { get; set; }
        public string AddOnYN { get; set; }
        public Nullable<short> Threshold { get; set; }
        public string UpgradeType { get; set; }
        public string languageid { get; set; }
    }
}
