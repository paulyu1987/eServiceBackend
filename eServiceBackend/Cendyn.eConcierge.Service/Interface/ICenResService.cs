using Cendyn.eConcierge.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.Service.CenResModel;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface ICenResService
    {
        string[] GetAvailableRoomTypes(string[] roomTypes, DateTime startDate, DateTime endDate, CenResParamsDTO cenResParams);
        List<RoomInfo> GetRoomInfos(string[] roomTypes, DateTime startDate, DateTime endDate, CenResParamsDTO cenResParams);
    }
}
