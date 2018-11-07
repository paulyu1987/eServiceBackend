using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Data
{
    public interface IConnectionStringBuilder
    {
        string GetConnectionString();
    }
}
