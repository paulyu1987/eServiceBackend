using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service.Interface
{
    public interface IAppConfig
    {
        string TripleDESEncryptionKey { get; }
        string ClientValidationEnabled { get; }
        string UnobtrusiveJavaScriptEnabled { get; }
    }
}
