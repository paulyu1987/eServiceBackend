using Cendyn.eConcierge.EntityModel;
using RepositoryT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Data.Repository.eConcierge.Interface
{
    public partial interface IRoomType_CodeRepository : IRepository<RoomType_Code>
    {
        IList<RoomTypeSearchResultDTO> GetRateTypeBySearchCriteria(RoomTypeSearchCriteriaDTO searchCriteria, PagingInformation pageInfo);
        RoomType_Code GetRoomTypeCode(string hotelCode, string roomCode, int languageID);
        bool IsStoredProcedureExists(string spname);
    }
}
