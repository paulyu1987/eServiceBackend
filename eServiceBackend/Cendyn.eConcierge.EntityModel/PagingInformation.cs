using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.EntityModel
{
    public class PagingInformation
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string OrderColumn { get; set; }
        public int RecordsTotal { get; set; }
        public int StartIndex { get; set; }
        public int RequestCommitted { get; set; }
        public string FilterValue { get; set; }
    }
}
