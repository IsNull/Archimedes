using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Drawing;
using Archimedes.Geometry.Units;

namespace Archimedes.Geometry
{
    public class Vertices : IList<Vector2>
    {
        readonly List<Vector2> _vertices;

        #region Constructor

        public Vertices() {
            _vertices = new List<Vector2>();
        }

        public Vertices(int capacity)
        {
            _vertices = new List<Vector2>(capacity);
        }

        public Vertices(IEnumerable<Vector2> vertices) {
            _vertices = new List<Vector2>(vertices);
        }

        public Vertices(IEnumerable<PointF> vertices) {
            _vertices = new List<Vector2>();
            this.AddRange(vertices);
        }

        #endregion

        #region To Methods

        public static IEnumerable<PointF> ToPointFs(IEnumerable<Vector2> vertices) {
            foreach (var v in vertices)
                yield return new PointF((float)v.X, (float)v.Y);
        }
        public static IEnumerable<Point> ToPoints(IEnumerable<Vector2> vertices) {
            foreach (var v in vertices)
                yield return new Point((int)v.X, (int)v.Y);
        }

        public IEnumerable<PointF> ToPoints()
        {
            return ToPointFs(_vertices);
        }

        public PointF[] ToPointArray() {
            return ToPoints().ToArray();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns a Boundingbox (orth rect) around all vertices
        /// </summary>
        public AARectangle BoundingBox
        {
            get
            {
                if (!_vertices.Any())
                    return AARectangle.Empty;

                var minX = (from v in _vertices
                            orderby v.X ascending
                            select v.X).First();

                var maxX = (from v in _vertices
                            orderby v.X descending
                            select v.X).First();

                var minY = (from v in _vertices
                            orderby v.Y ascending
                            select v.Y).First();

                var maxY = (from v in _vertices
                            orderby v.Y descending
                            select v.Y).First();

                return new AARectangle(minX, minY, (maxX - minX), (maxY - minY));
            }
        }

        #endregion

        public void AddRange(IEnumerable<Vector2> vertices) {
            _vertices.AddRange(vertices);
        }

        public void AddRange(IEnumerable<PointF> vertices) {
            foreach (var p in vertices)
                this.Add(p);
        }

        #region Public Modifiers/Creator Methods



        /// <summary>
        /// Rotates given vertices around a origin with given angle. New Vertices are returned.
        /// </summary>
        /// <param name="vertices">Array of Points</param>
        /// <param name="rotationOrigin">Origin location of rotation</param>
        /// <param name="angle">Rotation angle</param>
        /// <returns>New vertices array</returns>
        public Vertices RotateVertices(Vector2 rotationOrigin, Angle angle)
        {
            if (angle == Angle.Zero)
                return new Vertices(_vertices);

            var cnt = _vertices.Count();
            var rotVertices = new Vertices();
            foreach (var vertex in _vertices) {
                var vrotate = new Vector2(rotationOrigin, vertex);
                Vector2 pnt = rotationOrigin + vrotate.GetRotated(angle);
                rotVertices.Add(pnt);
            }
            return rotVertices;
        }

        #endregion

        #region IList

        public int IndexOf(Vector2 item) {
            return _vertices.IndexOf(item);
        }

        public void Insert(int index, Vector2 item) {
            _vertices.Insert(index, item);
        }

        public void RemoveAt(int index) {
            _vertices.RemoveAt(index);
        }


        public Vector2 this[int index] {
            get {
                return _vertices[index];
            }
            set {
                _vertices[index] = value;
            }
        }

        public void Add(Vector2 item) {
            _vertices.Add(item);
        }

        public void Clear() {
            _vertices.Clear();
        }

        public bool Contains(Vector2 item) {
            return _vertices.Contains(item);
        }

        public void CopyTo(Vector2[] array, int arrayIndex) {
            _vertices.CopyTo(array, arrayIndex);
        }

        public int Count {
            get { return _vertices.Count; }
        }

        public bool IsReadOnly {
            get { return false; /*_vertices.IsReadonly;*/ }
        }

        public bool Remove(Vector2 item) {
            return _vertices.Remove(item);
        }

        public IEnumerator<Vector2> GetEnumerator() {
            return _vertices.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return ((System.Collections.IEnumerable)_vertices).GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Moves all vertices by the given moving vector.
        /// </summary>
        /// <param name="mov">Translation vector</param>
        public void Translate(Vector2 mov)
        {
            for (int i = 0; i < _vertices.Count; i++)
            {
                _vertices[i] = _vertices[i] + mov;
            }
        }

        /// <summary>
        /// Scales all vertices by the given factor
        /// </summary>
        /// <param name="fact">Scale factor</param>
        public void Scale(double fact)
        {
            for (int i = 0; i < _vertices.Count; i++)
            {
                _vertices[i] = _vertices[i]*fact;
            }
        }
    }
}
