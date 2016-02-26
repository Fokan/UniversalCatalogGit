using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models.Repositories.Interfaces;

namespace WebApp.Models.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IItemRepository Items { get; }
        ITypeRepository Types { get; }
        IPropertyRepository Properties { get; }
        IDescriptionRepository Descriptions { get; }
        IImageRepository Images { get; }
        ICommentRepository Comments { get; }
        IUserRepository Users { get; }
        int SaveChanges();
    }
}
