using Cendyn.eConcierge.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface IUserManagementService
    {
        //int AddNewRateTypeCode(RateType rateType);
        //RateType GetRateTypeCodeByID(int ID);
        IList<UserSearchResultDTO> getUserBySearchCriteria(UserSearchCriteriaDTO searchCriteria, PagingInformation pageInfo);
        //string UpdateActiveYNByRateTypeID(int RateTypeID, string userName, int operation);

        IList<ListItemDTO> GetUserListByHotelCode(string hotelCode);
        //bool UpdateByUser(NewUserSaveDTO data);

        byte[] GenerateUserExcelBySearchCriteria(UserSearchCriteriaDTO searchCriteria, PagingInformation pageInfo);
    }
}
