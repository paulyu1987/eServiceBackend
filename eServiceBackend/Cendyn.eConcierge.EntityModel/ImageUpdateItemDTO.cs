using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class ImageUpdateItemDTO
    {
        public string HotelCode { get; set; }
        public string RoomCode { get; set; }
        public string FileName { get; set; }
        public string ChangedFileName { get; set; }
        public string ImageCaption { get; set; }
        public int LanguageID { get; set; }
        public int SortOrder { get; set; }
        public string Status { get; set; }
    }
}
