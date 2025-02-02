using Lab.Infrastructure.Context;
using Lab.Domain.Entities;
using Lab.Infrastructure.Repository.Orm_EF;
using Lab.Application.Interfaces;
using Lab.Application.Interfaces.Repo;
using Lab.Infrastructure.Repository.Orm_Dapper;

namespace Lab.Infrastructure.Repository
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly PostgresDbContext _context;

        private IGenericRepository<Product> _products;
        private IGenericRepository<Order> _orders;
        private IGenericRepository<OrderItem> _orderItems;
      
        private IUserRepository _users;
        private IRoleRepository _roles;
        private IUserAccessRepository _userAccess;

        private bool _disposed = false;
        public UnitOfWork(PostgresDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<Product> Products
        {
            get
            {
                return _products ??= new GenericRepository<Product>(_context);
            }
        }

        public IGenericRepository<Domain.Entities.Order> Orders
        {
            get
            {
                return _orders ??= new GenericRepository<Order>(_context);
            }
        }

        public IGenericRepository<OrderItem> OrderItems
        {
            get
            {
                return _orderItems ??= new GenericRepository<OrderItem>(_context);
            }
        }
        public IUserRepository Users
        {
            get
            {
                return _users ??= new UserRepository(_context);
            }
        }
        public IRoleRepository Roles 
        {
            get
            {
                return _roles ??= new RoleRepository(_context); 
            }
        }
        public IUserAccessRepository UserAccess
        {
            get
            {
                return _userAccess ??= new UserAccessRepository(_context); 
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                _disposed = true;
            }
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();   
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
