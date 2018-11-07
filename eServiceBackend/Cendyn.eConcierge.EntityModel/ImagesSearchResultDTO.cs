using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class ImagesSearchResultDTO
    {
        public int id { get; set; }
        public string Hotel_Code { get; set; }
        public string RoomCode { get; set; }
        public string RoomDescription { get; set; }
        public string RoomLongDescription { get; set; }
        public string UpgradeType { get; set; }
        public string AddOnYN { get; set; }
        public string ImageYN { get; set; }
        public string TotalRoom { get; set; }
        public string Threshold { get; set; }
        public string PriceDesc { get; set; }
        public bool IsActive { get; set; }
        public int ImagesCount { get; set; }
        public int? LanguageId { get; set; }
        public IList<RoomImage> Images { get; set; }
    }
}
