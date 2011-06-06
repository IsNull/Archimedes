// Finding a convex hull in the plane
// This program requires .Net version 2.0.
// Peter Sestoft (sestoft@itu.dk) * Java 2000-10-07, GC# 2001-10-27
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Archimedes.Geometry.Primitives;

namespace Archimedes.Geometry.Builder
{
    // ------------------------------------------------------------

    // Find the convex hull of a point set in the plane

    // An implementation of Graham's (1972) point elimination algorithm,
    // as modified by Andrew (1979) to find lower and upper hull separately.

    // This implementation correctly handles duplicate points, and
    // multiple points with the same x-coordinate.

    // 1. Sort the points lexicographically by increasing (x,y), thus 
    //    finding also a leftmost point L and a rightmost point R.
    // 2. Partition the point set into two lists, upper and lower, according as 
    //    point is above or below the segment LR.  The upper list begins with 
    //    L and ends with R; the lower list begins with R and ends with L.
    // 3. Traverse the point lists clockwise, eliminating all but the extreme
    //    points (thus eliminating also duplicate points).
    // 4. Eliminate L from lower and R from upper, if necessary.
    // 5. Join the point lists (in clockwise order) in an array.

    public class ConvexHullBuilder
    {
        public static Polygon2 Convexhull(IEnumerable<PointF> vertices) {

            if (vertices.Count() == 0)
                return new Polygon2();

            var pts = PointEx.ToPointExArray(vertices.ToArray());

            // Sort points lexicographically by increasing (x, y)
            int N = pts.Length;
            Polysort.Quicksort<PointEx>(pts);
            PointEx left = pts[0], right = pts[N - 1];
            // Partition into lower hull and upper hull
            CDLL<PointEx> lower = new CDLL<PointEx>(left), upper = new CDLL<PointEx>(left);
            for (int i = 0; i < N; i++) {
                double det = PointEx.Area2(left, right, pts[i]);
                if (det > 0)
                    upper = upper.Append(new CDLL<PointEx>(pts[i]));
                else if (det < 0)
                    lower = lower.Prepend(new CDLL<PointEx>(pts[i]));
            }
            lower = lower.Prepend(new CDLL<PointEx>(right));
            upper = upper.Append(new CDLL<PointEx>(right)).Next;
            // Eliminate points not on the hull
            eliminate(lower);
            eliminate(upper);
            // Eliminate duplicate endpoints
            if (lower.Prev.val.Equals(upper.val))
                lower.Prev.Delete();
            if (upper.Prev.val.Equals(lower.val))
                upper.Prev.Delete();
            // Join the lower and upper hull
            PointEx[] res = new PointEx[lower.Size() + upper.Size()];
            lower.CopyInto(res, 0);
            upper.CopyInto(res, lower.Size());
            return new Polygon2(PointEx.ToPointArray(res));
        }


        // Graham's scan
        private static void eliminate(CDLL<PointEx> start) {
            CDLL<PointEx> v = start, w = start.Prev;
            bool fwd = false;
            while (v.Next != start || !fwd) {
                if (v.Next == w)
                    fwd = true;
                if (PointEx.Area2(v.val, v.Next.val, v.Next.Next.val) < 0) // right turn
                    v = v.Next;
                else {                                       // left turn or straight
                    v.Next.Delete();
                    v = v.Prev;
                }
            }
        }
    }

    // ------------------------------------------------------------

    // Points in the plane

    class PointEx : Ordered<PointEx>
    {
        private static readonly Random rnd = new Random();

        public double x, y;

        public PointEx(double x, double y) {
            this.x = x; this.y = y;
        }

        public override string ToString() {
            return "(" + x + ", " + y + ")";
        }

        public static PointEx Random(int w, int h) {
            return new PointEx(rnd.Next(w), rnd.Next(h));
        }

        public bool Equals(PointEx p2) {
            return x == p2.x && y == p2.y;
        }

        public PointF ToPoint(){
            return new PointF((float)x, (float)y);
        }

        public static PointF[] ToPointArray(IEnumerable<PointEx> pexs) {
            return (from p in pexs
                    select (p.ToPoint())).ToArray();
        }
        public static PointEx[] ToPointExArray(IEnumerable<PointF> pexs) {
            return (from p in pexs
                    select (new PointEx(p.X,p.Y))).ToArray();
        }

        public override bool Less(Ordered<PointEx> o2) {
            PointEx p2 = (PointEx)o2;
            return x < p2.x || x == p2.x && y < p2.y;
        }

        // Twice the signed area of the triangle (p0, p1, p2)
        public static double Area2(PointEx p0, PointEx p1, PointEx p2) {
            return p0.x * (p1.y - p2.y) + p1.x * (p2.y - p0.y) + p2.x * (p0.y - p1.y);
        }
    }

    // ------------------------------------------------------------

    // Circular doubly linked lists of T

    class CDLL<T>
    {
        private CDLL<T> prev, next;     // not null, except in deleted elements
        public T val;

        // A new CDLL node is a one-element circular list
        public CDLL(T val) {
            this.val = val; next = prev = this;
        }

        public CDLL<T> Prev {
            get { return prev; }
        }

        public CDLL<T> Next {
            get { return next; }
        }

        // Delete: adjust the remaining elements, make this one point nowhere
        public void Delete() {
            next.prev = prev; prev.next = next;
            next = prev = null;
        }

        public CDLL<T> Prepend(CDLL<T> elt) {
            elt.next = this; elt.prev = prev; prev.next = elt; prev = elt;
            return elt;
        }

        public CDLL<T> Append(CDLL<T> elt) {
            elt.prev = this; elt.next = next; next.prev = elt; next = elt;
            return elt;
        }

        public int Size() {
            int count = 0;
            CDLL<T> node = this;
            do {
                count++;
                node = node.next;
            } while (node != this);
            return count;
        }

        public void PrintFwd() {
            CDLL<T> node = this;
            do {
                Console.WriteLine(node.val);
                node = node.next;
            } while (node != this);
            Console.WriteLine();
        }

        public void CopyInto(T[] vals, int i) {
            CDLL<T> node = this;
            do {
                vals[i++] = node.val;	// still, implicit checkcasts at runtime 
                node = node.next;
            } while (node != this);
        }
    }

    // ------------------------------------------------------------

    class Polysort
    {
        private static void swap<T>(T[] arr, int s, int t) {
            T tmp = arr[s]; arr[s] = arr[t]; arr[t] = tmp;
        }

        // Typed OO-style quicksort a la Hoare/Wirth

        private static void qsort<T>(Ordered<T>[] arr, int a, int b) {
            // sort arr[a..b]
            if (a < b) {
                int i = a, j = b;
                Ordered<T> x = arr[(i + j) / 2];
                do {
                    while (arr[i].Less(x)) i++;
                    while (x.Less(arr[j])) j--;
                    if (i <= j) {
                        swap<Ordered<T>>(arr, i, j);
                        i++; j--;
                    }
                } while (i <= j);
                qsort<T>(arr, a, j);
                qsort<T>(arr, i, b);
            }
        }

        public static void Quicksort<T>(Ordered<T>[] arr) {
            qsort<T>(arr, 0, arr.Length - 1);
        }
    }

    public abstract class Ordered<T>
    {
        public abstract bool Less(Ordered<T> that);
    }

    // ------------------------------------------------------------

}
