using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using Cendyn.eConcierge.EntityModel;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface IImportRoomDetailsService
    {
        List<int> Import(ISheet Sheet, IList<ListItemDTO> hotellist);
    }
}
