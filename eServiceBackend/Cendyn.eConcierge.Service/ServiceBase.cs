using Castle.Core.Logging;
using RepositoryT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cendyn.eConcierge.Service
{
    public class ServiceBase
    {
        private ILogger _logger = NullLogger.Instance;

        public ILogger logger
        {
            get
            {
                return _logger;
            }
            set
            {
                _logger = value;
            }
        }

        public IUnitOfWork unitOfWork { get; set; }

        public string GetCurrentClassName()
        {
            return this.GetType().Name;
        }
    }
}
