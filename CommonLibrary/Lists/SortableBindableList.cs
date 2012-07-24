using System.Collections.Generic;
using System.ComponentModel;
using CommonLibrary.Comparers;

namespace CommonLibrary.Lists
{
    public class SortableBindableList<T> : BindingList<T>
    {
        private ListSortDirection Direction
        {
            get;
            set;
        }

        private bool IsSorted
        {
            get;
            set;
        }

        protected override ListSortDirection SortDirectionCore
        {
            get
            {
                return Direction;
            }
        }

        protected override void RemoveSortCore()
        {
            IsSorted = false;
        }

        protected override bool SupportsSortingCore
        {
            get
            {
                return true;
            }
        }

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            PropertyComparer<T> comparer = new PropertyComparer<T>(prop, direction);

            List<T> items = (List<T>)Items;

            if (items != null)
            {
                items.Sort(comparer);

                Direction = direction;
                IsSorted = true;
            }
        }

        protected override bool IsSortedCore
        {
            get
            {
                return IsSorted;
            }
        }

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            new ListChangedEventArgs(ListChangedType.Reset, -1);
        }
    }
}