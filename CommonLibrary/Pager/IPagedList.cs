using System.Collections.Generic;

namespace CommonLibrary.Pager
{
    public interface IPagedList<T> : IEnumerable<T>
    {
        int Count { get; }

        int CurrentPage { get; }

        int FirstItemDisplayed { get; }

        bool IsFirstPage { get; }

        bool IsLastPage { get; }

        int ItemsPerPage { get; }

        int LastItemDisplayed { get; }

        int TotalItems { get; }

        int TotalPages { get; }

        T this[int index] { get; }

        IEnumerable<T> GetItemsOnPage();
    }
}