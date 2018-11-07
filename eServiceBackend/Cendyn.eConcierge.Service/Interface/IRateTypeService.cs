using Cendyn.eConcierge.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface IRateTypeService
    {
        int AddNewRateTypeCode(RateType rateType);
        RateType GetRateTypeCodeByID(int ID);
        IList<RateTypeSearchResultDTO> getRateTypeBySearchCriteria(RateTypeSearchCriteriaDTO searchCriteria, PagingInformation pageInfo);
        string UpdateActiveYNByRateTypeID(int RateTypeID, string userName, int operation);

        IList<ListItemDTO> GetRateTypeListByHotelCode(string hotelCode);
        bool UpdateByRateType(NewRateTypeSaveDTO data);

        byte[] GenerateRateTypeExcelBySearchCriteria(RateTypeSearchCriteriaDTO searchCriteria, PagingInformation pageInfo);
    }
}
