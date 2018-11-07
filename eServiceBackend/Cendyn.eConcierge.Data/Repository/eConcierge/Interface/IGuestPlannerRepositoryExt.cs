using Cendyn.eConcierge.EntityModel;
using RepositoryT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Data.Repository.eConcierge.Interface
{
    public partial interface IGuestPlannerRepository : IRepository<GuestPlanner>
    {
        bool UpdatRequestIDs(List<int> guestPlannerIDList);
        string GetRequestID(List<int> guestPlannerIDList);
        IList<RequestSearchResultDTO> GetRequestsBySearchCriteria(RequestSearchCriteriaDTO searchCriteria, PagingInformation pageInfo);
    }
}
