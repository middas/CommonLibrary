using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonLibrary.Pager
{
    public class PagedList<T> : IPagedList<T>
    {
        private List<T> _BackingList = new List<T>();

        public PagedList(IEnumerable<T> list, int currentPage, int itemsPerPage)
        {
            if (itemsPerPage < 1)
            {
                throw new Exception("Items per page cannot be less than 1");
            }

            if (currentPage < 1)
            {
                currentPage = 1;
            }

            if (list == null)
            {
                throw new ArgumentNullException("The backing list cannot be null");
            }

            _BackingList = list.ToList();
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;

            TotalItems = Count;
        }

        public PagedList(IEnumerable<T> list, int currentPage, int itemsPerPage, int totalItems)
            : this(list, currentPage, itemsPerPage)
        {
            TotalItems = totalItems;
        }

        public int Count
        {
            get { return _BackingList.Count; }
        }

        public int CurrentPage { get; protected set; }

        public int FirstItemDisplayed
        {
            get { return System.Math.Min(TotalItems, ((CurrentPage - 1) * ItemsPerPage) + 1); }
        }

        public bool IsFirstPage
        {
            get { return CurrentPage == 1; }
        }

        public bool IsLastPage
        {
            get { return CurrentPage == TotalPages; }
        }

        public int ItemsPerPage { get; protected set; }

        public int LastItemDisplayed
        {
            get { return System.Math.Min(TotalItems, FirstItemDisplayed + ItemsPerPage - 1); }
        }

        public int TotalItems { get; protected set; }

        public int TotalPages
        {
            get
            {
                int totalPages = (int)System.Math.Floor((double)TotalItems / (double)ItemsPerPage);

                if (TotalItems % ItemsPerPage > 0)
                {
                    totalPages++;
                }

                totalPages = System.Math.Max(1, totalPages);

                return totalPages;
            }
        }

        public T this[int index]
        {
            get { return _BackingList[index]; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _BackingList.GetEnumerator();
        }

        public IEnumerable<T> GetItemsOnPage()
        {
            IEnumerable<T> result;

            if (Count == TotalItems)
            {
                result = _BackingList.Skip(ItemsPerPage * (CurrentPage - 1)).Take(ItemsPerPage);
            }
            else
            {
                result = _BackingList;
            }

            return result;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}