using Cendyn.eConcierge.EntityModel;
using RepositoryT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Data.Repository.eConcierge.Interface
{
    public partial interface IEUpgradeRequestRepository : IRepository<eUpgradeRequest>
    {
        IList<ReportSearchResultDTO> GetReportBySearchCriteria(ReportSearchCriteriaDTO searchCriteria, PagingInformation pageInfo);
        IList<EUpgradeRequestDTO> GetEUpgradeRequestConfirmed();
        void UpDateeUpgradeRequest(string loginConfirmationNum, string hotelCode, string BookedRoomType, string UpgradeRoomType, string UpgradeStatus, string DecisionPerson);
    }
}
