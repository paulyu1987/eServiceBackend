using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Cendyn.eConcierge.Service.CenResModel
{
    public class RoomInfo
    {
        [JsonProperty("roomType")]
        public string RoomType { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("totalRooms")]
        public int TotalRooms { get; set; }
        [JsonProperty("availableRooms")]
        public int AvailableRooms { get; set; }
    }
}
