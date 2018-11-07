using Cendyn.eConcierge.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface ITransactionCodeService
    {
        IList<ListItemDTO> GeteUpgradeTransactionCodeListByHotelCode(string hotelCode);
        IList<HotelLanguageItem> GetLanguageListByHotelCode(string hotelCode);
        //string GetTransactionCode(string hotelCode, string upgradeType, bool perNightCharge);
    }
}