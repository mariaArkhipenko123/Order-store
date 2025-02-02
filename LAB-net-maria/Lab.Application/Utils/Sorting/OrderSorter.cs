using Lab.Domain.Entities;

namespace Lab.Application.Utils.Sorting
{
    public class OrderSorter
    {
        public static List<Order> SortOrdersByDate(List<Order> orders)
        {
            return MergeSort.Sort(orders, (order1, order2) =>
                order1.CreatedAt.CompareTo(order2.CreatedAt));
        }
    }
}
