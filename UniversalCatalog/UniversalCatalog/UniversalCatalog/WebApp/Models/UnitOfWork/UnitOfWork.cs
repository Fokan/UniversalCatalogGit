using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.Repositories;
using WebApp.Models.Repositories.Interfaces;

namespace WebApp.Models.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(/*ApplicationDbContext context*/)
        {
            //_context = context;
            _context = new ApplicationDbContext();
            Items = new ItemRepository(_context);
            Types = new TypeRepository(_context);
            Properties = new PropertyRepository(_context);
            Descriptions = new DescriptionRepository(_context);
            Images = new ImageRepository(_context);
            Comments = new CommentRepository(_context);
            Users = new UserRepository(_context);
            }

        public IItemRepository Items { get; private set; }
        public ITypeRepository Types { get; private set; }
        public IPropertyRepository Properties { get; private set; }
        public IDescriptionRepository Descriptions { get; private set; }
        public IImageRepository Images { get; private set; }
        public ICommentRepository Comments { get; private set; }
        public IUserRepository Users { get; private set; }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}