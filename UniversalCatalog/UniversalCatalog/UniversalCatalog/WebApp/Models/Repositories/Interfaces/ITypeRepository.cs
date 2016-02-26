using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Repositories.Interfaces
{
    public interface ITypeRepository : IRepository<Entities.Type>
    {
        bool IsThisTypeAsParent(Guid TypeId);
    }
}