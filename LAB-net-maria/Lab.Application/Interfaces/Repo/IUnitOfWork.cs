using Lab.Application.Interfaces.Repo;
using Lab.Domain.Entities;

namespace Lab.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<OrderItem> OrderItems { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<Product> Products { get; }
        IUserRepository Users { get; }
        void Dispose();
        Task SaveChangesAsync();
    }
}