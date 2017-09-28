using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace CommonLibrary.Comparers
{
    public class PropertyComparer<T> : Comparer<T>
    {
        private string _PropertyName;

        public PropertyComparer()
        {
        }

        public PropertyComparer(System.ComponentModel.PropertyDescriptor prop, System.ComponentModel.ListSortDirection direction)
        {
            _PropertyName = prop.Name;
            Property = typeof(T).GetProperty(prop.Name);
            Direction = direction;
        }

        private System.ComponentModel.ListSortDirection Direction
        {
            get;
            set;
        }

        private PropertyInfo Property
        {
            get;
            set;
        }

        public override int Compare(T x, T y)
        {
            if (x.GetType() == y.GetType())
            {
                int result;

                // handle dynamic types
                if (Property == null)
                {
                    Property = x.GetType().GetProperty(_PropertyName);
                }

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