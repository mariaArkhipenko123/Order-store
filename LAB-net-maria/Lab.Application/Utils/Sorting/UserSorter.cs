using Lab.Domain.Entities;

namespace Lab.Application.Utils.Sorting
{
    public class UserSorter
    {
        public static List<User> SortUsersByName(List<User> users)
        {
            return MergeSort.Sort(users, (user1, user2) =>
                string.Compare(user1.FirstName, user2.FirstName, StringComparison.Ordinal));
        }
    }
}
