using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.Data.Repository.CendynAdmin.Interface;
using System.Data.Entity.Core.EntityClient;

namespace Cendyn.eConcierge.Data
{
    public class ConnectionStringBuilder : IConnectionStringBuilder
    {
        private string _domainName;
        
        public ConnectionStringBuilder(string domainName)
        {
            _domainName = domainName;
        }

        public string GetConnectionString()
        {
            using (var context = new CendynAdminEntities())
            {
                //var config = context.eConcierge_Company.Where(p => p.DomainName_eConcierge == _domainName && p.Active == true).ToList().FirstOrDefault();
                var config = context.eUpgrade_Company.Where(p => p.DomainName_eUpgrade == _domainName && p.Active == true).ToList().FirstOrDefault();

                if (config != null)
                {
                    //string baseConnectionString = config.DotNetConnString_eConcierge;
                    string baseConnectionString = config.DotNetConnString_eUpgrade;

                    var entityBuilder = new EntityConnectionStringBuilder
                    {
                        Provider = "System.Data.SqlClient",
                        ProviderConnectionString = baseConnectionString,
                        Metadata =
                            String.Format(@"res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl", "ConciergeDB")
                    };

                    return entityBuilder.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
