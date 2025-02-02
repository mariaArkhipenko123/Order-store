namespace Lab.Application.Utils.Sorting
{
    public static class MergeSort
    {
        public static List<T> Sort<T>(IEnumerable<T> collection, Func<T, T, int> compare)
        {
            var list = new List<T>(collection);
            if (list.Count <= 1)
            {
                return list;
            }
            return MergeSortRecursive(list, compare);
        }

        private static List<T> MergeSortRecursive<T>(List<T> list, Func<T, T, int> compare)
        {
            if (list.Count <= 1)
            {
                return list;
            }

            int mid = list.Count / 2;
            var leftHalf = MergeSortRecursive(list.GetRange(0, mid), compare);
            var rightHalf = MergeSortRecursive(list.GetRange(mid, list.Count - mid), compare);

            return Merge(leftHalf, rightHalf, compare);
        }

        private static List<T> Merge<T>(List<T> left, List<T> right, Func<T, T, int> compare)
        {
            var sortedList = new List<T>();
            int leftIndex = 0, rightIndex = 0;

            while (leftIndex < left.Count && rightIndex < right.Count)
            {
                if (compare(left[leftIndex], right[rightIndex]) < 0)
                {
                    sortedList.Add(left[leftIndex]);
                    leftIndex++;
                }
                else
                {
                    sortedList.Add(right[rightIndex]);
                    rightIndex++;
                }
            }

            while (leftIndex < left.Count)
            {
                sortedList.Add(left[leftIndex]);
                leftIndex++;
            }

            while (rightIndex < right.Count)
            {
                sortedList.Add(right[rightIndex]);
                rightIndex++;
            }

            return sortedList;
        }
    }
}
