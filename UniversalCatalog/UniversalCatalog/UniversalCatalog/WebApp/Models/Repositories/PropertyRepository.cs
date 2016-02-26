using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.Entities;
using WebApp.Models.Repositories.Interfaces;

namespace WebApp.Models.Repositories
{
    public class PropertyRepository : Repository<Property>, IPropertyRepository
    {
        public PropertyRepository(ApplicationDbContext context) : base(context)
        {
        }

        public ApplicationDbContext UniversalCatalogContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}