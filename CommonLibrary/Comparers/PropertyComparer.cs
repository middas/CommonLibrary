using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace CommonLibrary.Comparers
{
    public class PropertyComparer<T> : Comparer<T>
    {
        private PropertyInfo Property
        {
            get;
            set;
        }

        private System.ComponentModel.ListSortDirection Direction
        {
            get;
            set;
        }

        public PropertyComparer()
        {
        }

        public PropertyComparer(System.ComponentModel.PropertyDescriptor prop, System.ComponentModel.ListSortDirection direction)
        {
            Property = typeof(T).GetProperty(prop.Name);
            Direction = direction;
        }

        public override int Compare(T x, T y)
        {
            if (x.GetType() == y.GetType())
            {
                int result;

                var xValue = Property.GetValue(x, null);
                var yValue = Property.GetValue(y, null);

                if (Direction == System.ComponentModel.ListSortDirection.Ascending)
                {
                    result = Comparer.Default.Compare(xValue, yValue);
                }
                else
                {
                    result = Comparer.Default.Compare(yValue, xValue);
                }

                return result;
            }
            else
            {
                throw new InvalidCastException("Invalid type passed");
            }
        }
    }
}
