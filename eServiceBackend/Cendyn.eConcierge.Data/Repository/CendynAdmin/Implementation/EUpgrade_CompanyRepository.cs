//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cendyn.eConcierge.Data.Repository.CendynAdmin.Implementation
{
    	
    using System;
    using System.Collections.Generic;
    using Cendyn.eConcierge.EntityModel;
    using Cendyn.eConcierge.Core.EntityFrameworkRepository;
    using Cendyn.eConcierge.Data.Repository.CendynAdmin.Interface;
    using RepositoryT.Infrastructure;
    
    
    public  partial class EUpgrade_CompanyRepository: EntityRepository<eUpgrade_Company,CendynAdminEntities>,IEUpgrade_CompanyRepository
    {
    	public EUpgrade_CompanyRepository(IServiceLocator serviceLocator)
                : base(serviceLocator)
            {
    
            }
    }
}