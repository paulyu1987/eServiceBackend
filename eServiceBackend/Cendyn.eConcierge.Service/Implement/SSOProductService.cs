using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cendyn.eConcierge.Service.Interface;
using Cendyn.eConcierge.Data.Repository.eConcierge.Interface;
using Cendyn.eConcierge.EntityModel;

namespace Cendyn.eConcierge.Service.Implement
{
    public class SSOProductService : ServiceBase, ISSOProductService
    {
        public ISSOProductRepository productRepo { get; set; }

        public SSOProduct GetProductByName(string productName)
        {
            return productRepo.Get(p => p.SSOProduct1 == productName);
        }
        public List<SSOProduct> GetProducts()
        {
            return productRepo.GetAll().Where(p => p.ActiveStatus == true && p.SSOProduct1 != "eUpgrade" && p.SSOProductURL != null).OrderBy(p=>p.SortOrder).ToList();
        }
    }
}
