using System;
using System.Collections.Generic;
using System.Linq;
using Archimedes.Geometry.Primitives;

namespace Archimedes.Geometry.Builder
{
    /// <summary>
    /// 
    /// Find the convex hull of a point set in the plane
    /// 
    /// An implementation of Graham's (1972) point elimination algorithm,
    /// as modified by Andrew (1979) to find lower and upper hull separately.
    /// 
    /// This implementation correctly handles duplicate points, and
    /// multiple points with the same x-coordinate.
    /// 
    /// 1. Sort the points lexicographically by increasing (x,y), thus 
    ///    finding also a leftmost point L and a rightmost point R.
    /// 2. Partition the point set into two lists, upper and lower, according as
    ///    point is above or below the segment LR.  The upper list begins with 
    ///    L and ends with R; the lower list begins with R and ends with L.
    /// 3. Traverse the point lists clockwise, eliminating all but the extreme
    ///    points (thus eliminating also duplicate points).
    /// 4. Eliminate L from lower and R from upper, if necessary.
    /// 5. Join the point lists (in clockwise order) in an array.
    /// 
    ///
    /// Code bases upon Peter Sestoft (sestoft@itu.dk) reference implementation
    /// * Java 2000-10-07
    /// 
    /// Further modifications and tuning by P. Büttiker 2010-2012
    /// 
    /// </summary>
    public class ConvexHullBuilder
    {
        public static Polygon2 Convexhull(IEnumerable<Vector2> vertices) {
            var pts = vertices.ToArray();

            if (!pts.Any())
                return new Polygon2();

            

            // Sort points lexicographically by increasing (x, y)
            int N = pts.Length;
            Polysort.Quicksort(pts);


            var left = pts[0];
            var right = pts[N - 1];

            // Partition into lower hull and upper hull
            var lower = new CircularDoublyLinkedList<Vector2>(left);
            var upper = new CircularDoublyLinkedList<Vector2>(left);

            for (int i = 0; i < N; i++) {
                double det = Vector2.Area2(left, right, pts[i]);
                if (det > 0)
                    upper = upper.Append(new CircularDoublyLinkedList<Vector2>(pts[i]));
                else if (det < 0)
                    lower = lower.Prepend(new CircularDoublyLinkedList<Vector2>(pts[i]));
            }
            lower = lower.Prepend(new CircularDoublyLinkedList<Vector2>(right));
            upper = upper.Append(new CircularDoublyLinkedList<Vector2>(right)).Next;
            // Eliminate points not on the hull
            Eliminate(lower);
            Eliminate(upper);
            // Eliminate duplicate endpoints
            if (lower.Prev.val.Equals(upper.val))
                lower.Prev.Delete();
            if (upper.Prev.val.Equals(lower.val))
                upper.Prev.Delete();
            // Join the lower and upper hull
            var res = new Vector2[lower.Size() + upper.Size()];
            lower.CopyInto(res, 0);
            upper.CopyInto(res, lower.Size());
            return new Polygon2(res);
        }


        // Graham's scan
        private static void Eliminate(CircularDoublyLinkedList<Vector2> start)
        {
            CircularDoublyLinkedList<Vector2> v = start, w = start.Prev;
            bool fwd = false;
            while (v.Next != start || !fwd) {
                if (v.Next == w)
                    fwd = true;
                if (Vector2.Area2(v.val, v.Next.val, v.Next.Next.val) < 0) // right turn
                    v = v.Next;
                else {                                       // left turn or straight
                    v.Next.Delete();
                    v = v.Prev;
                }
            }
        }
    }

    /// <summary>
    /// Circular doubly linked lists of T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class CircularDoublyLinkedList<T>
    {
        public T val;

        // A new CircularDoublyLinkedList node is a one-element circular list
        public CircularDoublyLinkedList(T val) {
            this.val = val; Next = Prev = this;
        }

        public CircularDoublyLinkedList<T> Prev { get; private set; }
        public CircularDoublyLinkedList<T> Next { get; private set; }

        /// <summary>
        /// Adjust the remaining elements, make this one point nowhere
        /// </summary>
        public void Delete() {
            Next.Prev = Prev; Prev.Next = Next;
            Next = Prev = null;
        }

        public CircularDoublyLinkedList<T> Prepend(CircularDoublyLinkedList<T> elt) {
            elt.Next = this; elt.Prev = Prev; Prev.Next = elt; Prev = elt;
            return elt;
        }

        public CircularDoublyLinkedList<T> Append(CircularDoublyLinkedList<T> elt) {
            elt.Prev = this; elt.Next = Next; Next.Prev = elt; Next = elt;
            return elt;
        }

        public int Size() {
            int count = 0;
            var node = this;
            do {
                count++;
                node = node.Next;
            } while (node != this);
            return count;
        }

        public void PrintFwd() {
            var node = this;
            do {
                Console.WriteLine(node.val);
                node = node.Next;
            } while (node != this);
            Console.WriteLine();
        }

        public void CopyInto(T[] vals, int i) {
            var node = this;
            do {
                vals[i++] = node.val;	// still, implicit checkcasts at runtime 
                node = node.Next;
            } while (node != this);
        }
    }

    #region Polysort

    static class Polysort
    {
        private static void Swap<T>(T[] arr, int s, int t) {
            var tmp = arr[s]; arr[s] = arr[t]; arr[t] = tmp;
        }

        private static void Qsort<T>(T[] arr, int a, int b) 
            where T : IOrdered<T>{
            // sort arr[a..b]
            if (a < b) {
                int i = a, j = b;
                var x = arr[(i + j) / 2];
                do {
                    while (arr[i].Less(x)) i++;
                    while (x.Less(arr[j])) j--;
                    if (i <= j) {
                        Swap(arr, i, j);
                        i++; j--;
                    }
                } while (i <= j);
                Qsort<T>(arr, a, j);
                Qsort<T>(arr, i, b);
            }
        }

        /// <summary>
        /// Quicksort implementation 
        /// Hoare/Wirth
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        public static void Quicksort<T>(T[] arr) 
            where T : IOrdered<T>
        {
            Qsort<T>(arr, 0, arr.Length - 1);
        }
    }

    #endregion
}
