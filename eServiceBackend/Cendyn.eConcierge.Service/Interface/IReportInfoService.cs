using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.EntityModel;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface IReportInfoService
    {
        IList<ReportSearchResultDTO> GetReportBySearchCriteria(ReportSearchCriteriaDTO searchCriteria, PagingInformation pageInfo);//, string DateFormatForALL);

        byte[] GenerateReportExcelBySearchCriteria(ReportSearchCriteriaDTO searchCriteria, PagingInformation pageInfo);//, string DateFormatForALL);
    }
}
