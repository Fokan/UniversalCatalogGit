using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.Repositories.Interfaces;

namespace WebApp.Models.Repositories
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public ApplicationDbContext UniversalCatalogContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}