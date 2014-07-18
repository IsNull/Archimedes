using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Archimedes.Patterns.Data.Sorting
{
    /// <summary>
    /// Implements natural string sorting algorythm
    /// </summary>
    public class NaturalSortComparer : IComparer<string>, IDisposable
    {
        // Cache table used for subsequent calls to Compare
        private Dictionary<string, string[]> _table = new Dictionary<string, string[]>();


        public NaturalSortComparer(ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            SortDirection = sortDirection;
        }

        /// <summary>
        /// 
        /// </summary>
        public ListSortDirection SortDirection { get; set; } 

        /// <summary>
        /// Returns -1, 0 or 1.
        /// Return value Less than zero means x is less than y.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(string x, string y)
        {
            // Handle base cases and prevent null pointers

            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            string[] x1, y1;

            if (!_table.TryGetValue(x, out x1))
            {
                x1 = Regex.Split(x.Replace(" ", ""), "([0-9]+)");
                _table.Add(x, x1);
            }

            if (!_table.TryGetValue(y, out y1))
            {
                y1 = Regex.Split(y.Replace(" ", ""), "([0-9]+)");
                _table.Add(y, y1);
            }

            int returnVal;

            for (int i = 0; i < x1.Length && i < y1.Length; i++)
            {
                if (x1[i] != y1[i])
                {
                    returnVal = PartCompare(x1[i], y1[i]);
                    return SortDirection == ListSortDirection.Ascending ? returnVal : -returnVal;
                }
            }

            if (y1.Length > x1.Length)
            {
                returnVal = 1;
            }
            else if (x1.Length > y1.Length)
            {
                returnVal = -1;
            }
            else
            {
                returnVal = 0;
            }

            return SortDirection == ListSortDirection.Ascending ? returnVal : -returnVal;
        }

        private static int PartCompare(string left, string right)
        {
            int x, y;
            if (!int.TryParse(left, out x))
                return left.CompareTo(right);

            if (!int.TryParse(right, out y))
                return left.CompareTo(right);

            return x.CompareTo(y);
        }

        public void Dispose()
        {
            _table.Clear();
            _table = null;
        }
    }


}
