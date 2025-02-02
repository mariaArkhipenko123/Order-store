using Lab.Application.Utils.Sorting;
using Xunit;

namespace Test
{
    public class MergeSortTests
    {
        [Fact]
        public void SimpleMergeSort()
        {
            var unsorted = new List<int> { 5, 3, 8, 1, 2 };
            var expected = new List<int> { 1, 2, 3, 5, 8 };
            Func<int, int, int> compare = (x, y) => x.CompareTo(y);

            var sorted = MergeSort.Sort(unsorted, compare);

            Assert.Equal(expected, sorted);
        }

        [Fact]
        //We check that the Sort method returns the same list if it is given an already sorted list as input.
        public void Sort_ShouldReturnSameList() 
        { 
            var sorted = new List<int> { 1, 2, 3, 4, 5 };
            var expected = new List<int> { 1, 2, 3, 4, 5 };
            Func<int, int, int> compare = (x, y) => x.CompareTo(y);
 
            var result = MergeSort.Sort(sorted, compare);
 
            Assert.Equal(expected, result);
        }

        [Fact]
        //We check that the method correctly sorts the list with duplicate values.
        public void Sort_ShouldReturnSortedList()
        { 
            var unsorted = new List<int> { 5, 3, 8, 3, 1, 2 };
            var expected = new List<int> { 1, 2, 3, 3, 5, 8 };
            Func<int, int, int> compare = (x, y) => x.CompareTo(y);

            var sorted = MergeSort.Sort(unsorted, compare);
 
            Assert.Equal(expected, sorted);
        }
    }
}
