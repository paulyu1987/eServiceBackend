using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;
using Cendyn.eConcierge.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.Implement
{
    public class RoomImageService : ServiceBase, IRoomImageService
    {
        public IRoomImageRepository imagesRepo { get; set; }
        public IList<RoomImage> GetRoomImagesByHotelAndRoomCode(string hotelCode, string roomCode)
        {
            List<RoomImage> list = imagesRepo.GetAll().Where(x => x.HotelCode == hotelCode&&x.RoomCode==roomCode).ToList();
            return list;
        }

    }
}
