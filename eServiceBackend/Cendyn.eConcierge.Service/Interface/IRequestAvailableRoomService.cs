﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.EntityModel;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface IRequestAvailableRoomService
    {
        IList<AvailableRoomDTO> GetGuestInfo(int requestID);      
    }
}
