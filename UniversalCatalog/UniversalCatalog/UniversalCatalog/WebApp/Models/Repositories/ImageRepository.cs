using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.Repositories.Interfaces;

namespace WebApp.Models.Repositories
{
    public class ImageRepository : Repository<Entities.Image>, IImageRepository
    {
        public ImageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public ApplicationDbContext UniversalCatalogContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}