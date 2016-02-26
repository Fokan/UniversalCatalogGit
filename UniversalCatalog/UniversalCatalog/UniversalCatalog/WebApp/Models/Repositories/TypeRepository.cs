using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models.Repositories.Interfaces;

namespace WebApp.Models.Repositories
{
    public class TypeRepository : Repository<Entities.Type>, ITypeRepository
    {
        public TypeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public ApplicationDbContext UniversalCatalogContext
        {
            get { return Context as ApplicationDbContext; }
        }

        public IEnumerable<Entities.Type> GetTypesWhichHaventParentType()
        {
            return UniversalCatalogContext.Types.Where(type => type.ParentType == null).ToList();
        }
        
        public bool IsThisTypeAsParent(Guid TypeId)
        {
            return !(Count(t => t.ParentTypeId == TypeId) == 0);
        }
    }
}