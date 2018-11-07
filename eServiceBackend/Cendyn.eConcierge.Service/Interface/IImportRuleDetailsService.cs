using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using Cendyn.eConcierge.EntityModel;
using System.Collections;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface IImportRuleDetailsService
    {
        List<ListItemInt> Import(ISheet Sheet, IList<ListItemDTO> hotellist);
    }
}
