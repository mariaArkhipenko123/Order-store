using Lab.Domain.Entities;

namespace Lab.Application.Utils.Sorting
{
    public class ProductSorter
    {
        public static List<Product> SortProductsByPrice(List<Product> products)
        {
            return MergeSort.Sort(products, (product1, product2) =>
                product1.Price.CompareTo(product2.Price));
        }
    }
}
