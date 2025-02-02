using HotChocolate;
using HotChocolate.Data;
using Lab.Application.Interfaces;
using Lab.Application.Interfaces.Repo;
using Lab.Domain.Entities;

namespace Lab.Application.GraphQL
{
    public class GraphQueries
    {
        [UseProjection]
        [UseFiltering()]
        [UseSorting()]
        public async Task<IEnumerable<User>> GetAllUsersAsync([Service] IUserRepository userRepository)
        {
            return await userRepository.GetAllAsync();
        }
         [UseProjection]
        [UseFiltering()]
        [UseSorting()]
        public async Task<IEnumerable<Order>> ReadOrdersAsync([Service] IGenericRepository<Order> orderRepository)
        {
            return await orderRepository.GetAllAsync();
        }

        [UseProjection]
        [UseFiltering()]
        [UseSorting()]
        public async Task<IEnumerable<OrderItem>> ReadOrderItemsAsync([Service] IGenericRepository<OrderItem> orderItemRepository)
        {
            return await orderItemRepository.GetAllAsync();
        }

        [UseProjection]
        [UseFiltering()]
        [UseSorting()]
        public async Task<IEnumerable<Product>> ReadProductsAsync([Service] IGenericRepository<Product> productRepository)
        {
            return await productRepository.GetAllAsync();
        }
    }
}
