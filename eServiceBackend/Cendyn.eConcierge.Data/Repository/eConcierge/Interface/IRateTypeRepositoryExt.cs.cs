using Cendyn.eConcierge.EntityModel;
using RepositoryT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cendyn.eConcierge.Data.Repository.eConcierge.Interface
{
    public  partial interface IRateTypeRepository : IRepository<RateType>
    {
        IList<RateTypeSearchResultDTO> GetRateTypeBySearchCriteria(RateTypeSearchCriteriaDTO searchCriteria, PagingInformation pageInfo);
    }
}
