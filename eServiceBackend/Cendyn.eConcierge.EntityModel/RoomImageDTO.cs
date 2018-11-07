using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class RoomImageDTO
    {
        public int ID { get; set; }
        public string HotelCode { get; set; }
        public string RoomCode { get; set; }
        public string FileName { get; set; }
        public string ImageCaption { get; set; }
        public string ImageUrl { get; set; }
        public Nullable<int> LanguageID { get; set; }
        public Nullable<int> SortOrder { get; set; }
    }
}
