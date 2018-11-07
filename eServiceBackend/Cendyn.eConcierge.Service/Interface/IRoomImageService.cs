using Cendyn.eConcierge.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface IRoomImageService
    {
        IList<RoomImage> GetRoomImagesByHotelAndRoomCode(string hotelCode, string roomCode);
    }
}
