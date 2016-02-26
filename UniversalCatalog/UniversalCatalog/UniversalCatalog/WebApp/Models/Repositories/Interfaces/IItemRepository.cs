using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models.Entities;

namespace WebApp.Models.Repositories.Interfaces
{
    public interface IItemRepository : IRepository<Item>
    {
        IEnumerable<Item> GetPartOfItems(int pageNumber, int pageSize);
        IEnumerable<Item> GetPartOfItemsByType(int pageNumber, int pageSize, Guid TypeId);
    }
}
