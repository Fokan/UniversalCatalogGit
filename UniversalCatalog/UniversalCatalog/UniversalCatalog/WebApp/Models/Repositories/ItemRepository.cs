using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.Entities;
using WebApp.Models.Repositories.Interfaces;

namespace WebApp.Models.Repositories
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(ApplicationDbContext context):base(context)
        {
        }

        public ApplicationDbContext UniversalCatalogContext
        {
            get { return Context as ApplicationDbContext; }
        }

        public IEnumerable<Item> GetPartOfItems(int pageNumber, int pageSize)
        {
            return UniversalCatalogContext.Items.OrderBy(item=>item.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public IEnumerable<Item> GetPartOfItemsByType(int pageNumber, int pageSize, Guid TypeId)
        {
            return UniversalCatalogContext.Items.Where(item=>item.TypeId == TypeId).OrderBy(item => item.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }


    }
}