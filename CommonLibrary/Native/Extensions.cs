﻿using CommonLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace CommonLibrary.Native
{
    public static class Extensions
    {
        #region Int32s

        public static string ToWord(this int i)
        {
            return IntToWord.ConvertIntToWords(i);
        }

        #endregion Int32s

        #region IntPtr

        public static T AsStruct<T>(this IntPtr i)
        {
            return (T)Marshal.PtrToStructure(i, typeof(T));
        }

        #endregion IntPtr

        #region Strings

        public static bool ContainsIgnoreCase(this string s, string value)
        {
            if (value.IsNullOrEmpty())
            {
                throw new ArgumentNullException("value cannot be null");
            }

            return s.ToUpper().Contains(value.ToUpper());
        }

        public static T Deserialize<T>(this string value) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            T newObject;

            using (StringReader stringReader = new StringReader(value))
            {
                using (XmlReader xmlReader = new XmlTextReader(stringReader))
                {
                    try
                    {
                        newObject = serializer.Deserialize(xmlReader) as T;
                    }
                    catch
                    {
                        newObject = null;
                    }
                }
            }

            return newObject;
        }

        public static bool EqualsIgnoreCase(this string s, string value)
        {
            if (value.IsNullOrEmpty())
            {
                throw new ArgumentNullException("value cannot be null");
            }

            return s.ToUpper().Equals(value.ToUpper());
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool IsNullOrEmptyTrim(this string s)
        {
            bool isNullOrEmpty = false;

            if (!s.IsNullOrEmpty())
            {
                isNullOrEmpty = string.IsNullOrEmpty(s.Trim());
            }
            else
            {
                isNullOrEmpty = s.IsNullOrEmpty();
            }

            return isNullOrEmpty;
        }

        public static string JoinIgnoreNullOrEmpty(this string s, string separator, params string[] values)
        {
            if (values != null && values.Length > 0)
            {
                string combinedValues = values.Aggregate((x, y) => y.IsNullOrEmptyTrim() ? x : x.IsNullOrEmptyTrim() ? y : string.Concat(x, separator, y));

                if (!combinedValues.IsNullOrEmptyTrim())
                {
                    s = s.IsNullOrEmptyTrim() ? combinedValues : string.Concat(s, separator, combinedValues);
                }
            }

            return s;
        }

        public static string SFormat(this string s, params object[] args)
        {
            return string.Format(s, args);
        }

        #endregion Strings

        #region Object

        public static string SerializeXML<T>(this T value) where T : class
        {
            string xml = string.Empty;

            var serializer = new XmlSerializer(typeof(T));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.Serialize(memoryStream, value);

                memoryStream.Position = 0;

                using (StreamReader reader = new StreamReader(memoryStream))
                {
                    xml = reader.ReadToEnd();
                }
            }

            return xml;
        }

        public static decimal? ToDecimalOrDefault(this object s)
        {
            decimal d;
            if (s != null && Decimal.TryParse(s.ToString(), out d))
            {
                return d;
            }
            return null;
        }

        public static decimal ToDecimalOrDefault(this object s, decimal x)
        {
            decimal? d;
            if ((d = s.ToDecimalOrDefault()) != null)
            {
                return d.Value;
            }
            return x;
        }

        public static decimal? ToDecimalOrDefault(this object s, bool containsSpecialCharacters)
        {
            string value = s.ToString();

            if (containsSpecialCharacters)
            {
                value = string.Empty;

                Regex regex = new Regex(@"[\d.]");
                var matches = regex.Matches(s.ToString());

                foreach (var match in matches)
                {
                    value = value.JoinIgnoreNullOrEmpty("", match.ToString());
                }

                s = value;
            }
            return s.ToDecimalOrDefault();
        }

        public static decimal ToDecimalOrDefault(this object s, bool containsSpecialCharacters, decimal x)
        {
            decimal? d;
            if ((d = s.ToDecimalOrDefault(containsSpecialCharacters)) != null)
            {
                return d.Value;
            }
            return x;
        }

        public static double? ToDoubleOrDefault(this object s)
        {
            double d;
            if (s != null && Double.TryParse(s.ToString(), out d))
            {
                return d;
            }
            return null;
        }

        public static double ToDoubleOrDefault(this object s, double x)
        {
            double? d;
            if ((d = s.ToDoubleOrDefault()) != null)
            {
                return d.Value;
            }
            return x;
        }

        public static float? ToFloatOrDefault(this object s)
        {
            float f;
            if (s != null && float.TryParse(s.ToString(), out f))
            {
                return f;
            }
            return null;
        }

        public static float ToFloatOrDefault(this object s, float x)
        {
            float? f;
            if ((f = s.ToFloatOrDefault()) != null)
            {
                return f.Value;
            }
            return x;
        }

        public static int? ToIntegerOrDefault(this object s)
        {
            int i;
            if (s != null && Int32.TryParse(s.ToString(), out i))
            {
                return i;
            }
            return null;
        }

        public static int ToIntegerOrDefault(this object s, int x)
        {
            int? i;
            if ((i = s.ToIntegerOrDefault()) != null)
            {
                return i.Value;
            }
            return x;
        }

        public static long? ToLongOrDefault(this object s)
        {
            long i;
            if (s != null && Int64.TryParse(s.ToString(), out i))
            {
                return i;
            }

            return null;
        }

        public static long ToLongOrDefault(this object s, long x)
        {
            long? i;
            if ((i = s.ToLongOrDefault()) != null)
            {
                return i.Value;
            }

            return x;
        }

        #endregion Object

        #region IEnumerable<T>

        public static bool ContainsProperty<T>(this IEnumerable<T> source, Func<T, bool> comparer)
        {
            if (comparer == null || source == null)
            {
                throw new ArgumentNullException("Arguments cannot be null");
            }

            bool containsProperty = false;

            foreach (T item in source)
            {
                if (comparer(item))
                {
                    containsProperty = true;
                    break;
                }
            }

            return containsProperty;
        }

        public static int ContainsPropertyCount<T>(this IEnumerable<T> source, Func<T, bool> comparer)
        {
            if (comparer == null || source == null)
            {
                throw new ArgumentNullException("Arguments cannot be null");
            }

            int count = 0;

            foreach (T item in source)
            {
                if (comparer(item))
                {
                    count++;
                }
            }

            return count;
        }

        public static int CountUntil<T>(this IEnumerable<T> source, Func<T, bool> comparer)
        {
            int count = 0;

            foreach (T obj in source)
            {
                if (comparer(obj))
                {
                    break;
                }
                else
                {
                    count++;
                }
            }

            return count;
        }

        public static IEnumerable<T> DistinctBy<T, K>(this IEnumerable<T> source, Expression<Func<T, K>> keySelector)
        {
            if (keySelector == null || source == null)
            {
                throw new ArgumentNullException("Arguments cannot be null");
            }

            HashSet<K> knownKeys = new HashSet<K>();

            foreach (T item in source)
            {
                var value = keySelector.Compile()(item);

                if (value != null && knownKeys.Add(value))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<T> ExecuteForEach<U, T>(this IEnumerable<U> source, Func<U, T> function)
        {
            foreach (U item in source)
            {
                yield return function(item);
            }
        }

        public static void PerformOnEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }
        }

        public static bool PropertyMatchesAll<T>(this IEnumerable<T> source, Func<T, bool> comparer)
        {
            if (comparer == null || source == null)
            {
                throw new ArgumentNullException("Arguments cannot be null");
            }

            bool matches = true;

            foreach (T item in source)
            {
                if (!comparer(item))
                {
                    matches = false;
                    break;
                }
            }

            return matches;
        }

        public static IEnumerable<T> RemoveWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach (T item in source)
            {
                if (!predicate(item))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            List<T> oldList = source.ToList();
            Random random = new Random();

            while (oldList.Count > 0)
            {
                int index = random.Next(0, oldList.Count);

                T t = oldList[index];

                oldList.RemoveAt(index);

                yield return t;
            }
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("Source cannot be null");
            }

            PropertyInfo[] propertyInfo = typeof(T).GetProperties(BindingFlags.Public);

            DataTable table = new DataTable();

            //set columns
            foreach (PropertyInfo property in propertyInfo)
            {
                Type colType = property.PropertyType;

                if (colType.IsGenericType && colType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    colType = colType.GetGenericArguments()[0];
                }

                table.Columns.Add(property.Name, colType);
            }

            //set data
            foreach (T item in source)
            {
                var row = table.NewRow();

                foreach (PropertyInfo property in propertyInfo)
                {
                    row[property.Name] = property.GetValue(item, null) ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }

            return table;
        }

        public static string ToString<T>(this IEnumerable<T> source, Func<T, string> stringElement, string separator)
        {
            List<string> stringCollection = source.ExecuteForEach(t => stringElement(t)).ToList();

            return string.Join(separator, stringCollection.ToArray());
        }

        public static string ToString<T>(this IEnumerable<T> source, string separator)
        {
            return source.ToString(t => t.ToString(), separator);
        }

        #endregion IEnumerable<T>

        #region DataView

        public static string GetDelimitedString(this DataView source, string rowDelimiter, string columnDelimiter, bool includeHeaders, params string[] columns)
        {
            List<string> rowData = new List<string>();
            List<string> headerData = new List<string>();

            foreach (DataColumn column in source.Table.Columns)
            {
                if (columns == null || columns.Length == 0)
                {
                    headerData.Add(column.ColumnName);
                }
                else if (columns.Contains(column.ColumnName))
                {
                    headerData.Add(column.ColumnName);
                }
            }

            foreach (DataRowView row in source)
            {
                List<string> cellData = new List<string>();

                foreach (string header in headerData)
                {
                    cellData.Add(row[header].ToString());
                }

                rowData.Add(cellData.ToString(columnDelimiter));
            }

            if (includeHeaders)
            {
                rowData.Insert(0, headerData.ToString(columnDelimiter));
            }

            return rowData.ToString(rowDelimiter);
        }

        #endregion DataView

        #region DataTable

        public static string GetDelimitedString(this DataTable source, string rowDelimiter, string columnDelimiter, bool includeHeaders, params string[] columns)
        {
            return source.DefaultView.GetDelimitedString(rowDelimiter, columnDelimiter, includeHeaders, columns);
        }

        public static List<U> ToList<U>(this DataTable source, Func<DataRow, U> function)
        {
            List<U> list = new List<U>();

            foreach (DataRow row in source.Rows)
            {
                list.Add(function(row));
            }

            return list;
        }

        #endregion DataTable

        #region ControlCollection

        public static IEnumerable<T> ToList<T>(this System.Windows.Forms.Control.ControlCollection source)
        {
            var enumerator = source.GetEnumerator();

            while (enumerator.MoveNext())
            {
                yield return (T)enumerator.Current;
            }
        }

        public static IEnumerable<T> ToList<T>(this System.Windows.Forms.Form.ControlCollection source)
        {
            var enumerator = source.GetEnumerator();

            while (enumerator.MoveNext())
            {
                yield return (T)enumerator.Current;
            }
        }

        #endregion ControlCollection

        #region Control

        public static void Invoke(this Control control, Action action)
        {
            control.Invoke((Delegate)action);
        }

        #endregion Control

        #region Null Helper

        public static bool IsNull<T>(this T root, Expression<Func<T, object>> getter)
        {
            IsNullVisitor visitor = new IsNullVisitor();

            visitor.CurrentObject = root;
            visitor.Visit(getter);

            return visitor.IsNull;
        }

        private class IsNullVisitor : ExpressionVisitor
        {
            public object CurrentObject;

            public bool IsNull { get; set; }

            protected override Expression VisitMember(MemberExpression node)
            {
                base.VisitMember(node);

                // before attempting to get the value, check for nulls or this will throw an exception
                if (CheckNull())
                {
                    return node;
                }

                PropertyInfo member = (PropertyInfo)node.Member;

                CurrentObject = member.GetValue(CurrentObject, null);

                CheckNull();

                return node;
            }

            private bool CheckNull()
            {
                if (CurrentObject == null)
                {
                    IsNull = true;
                }

                return IsNull;
            }
        }

        #endregion Null Helper
    }
}