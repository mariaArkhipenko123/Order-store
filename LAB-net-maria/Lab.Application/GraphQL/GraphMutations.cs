using HotChocolate;
using Lab.Application.Interfaces;
using Lab.Application.Interfaces.Repo;
using Lab.Application.Models.DTOs;
using Lab.Application.Models.DTOs.GraphQL;
using Lab.Domain.Entities;

namespace Lab.Application.GraphQL
{
    public class GraphMutations
    {
        public async Task<OrderDto> CreateOrder(OrderDto orderDto,
            [Service] IGenericRepository<Order> orderRepository,
            [Service] IGenericRepository<OrderItem> orderItemRepository,
            [Service] IUnitOfWork unitOfWork)
        {
            var order = new Order
            {
                UserId = orderDto.UserId,
                TotalPrice = orderDto.TotalPrice,
                OrderItems = new List<OrderItem>()
            };

            await orderRepository.AddAsync(order);

            foreach (var item in orderDto.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,   
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };
                await orderItemRepository.AddAsync(orderItem);  
                order.OrderItems.Add(orderItem);  
            }

            await unitOfWork.SaveChangesAsync(); 

            var result = new OrderDto
            {
                UserId = order.UserId,
                TotalPrice = order.TotalPrice,
                Items = order.OrderItems.Select(io => new OrderItemDto
                {
                    ProductId = io.ProductId,
                    Quantity = io.Quantity
                }).ToList()
            };

            return result; 
        }
    }
}

