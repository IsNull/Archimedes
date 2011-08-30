using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Geometry.Rendering
{
    /// <summary>
    /// Threadsafe Drawing Group
    /// </summary>
    public class DrawingGroup : IDrawable
    {
        #region Fields

        readonly List<IDrawable> _grpdrawings = new List<IDrawable>();
        readonly object _drawingsSYNC = new object();

        //readonly List<Exception> _drawingErrors = new List<Exception>();

        #endregion

        public string Name { get; set; }

        public DrawingGroup(string uname) {
            Name = uname;
        }

        #region Drawable Handling

        public void Add(IDrawable d) {
            lock (_drawingsSYNC) {
                _grpdrawings.Add(d);
            }
        }

        public void AddRange(IEnumerable<IDrawable> d) {
            lock (_drawingsSYNC) {
                _grpdrawings.AddRange(d);
            }
        }


        /// <summary>
        /// Remove the given Element
        /// </summary>
        /// <param name="d"></param>
        public void Remove(IDrawable d) {
            lock (_drawingsSYNC) {
                _grpdrawings.Remove(d);
            }
        }

        /// <summary>
        /// Clear all Elements
        /// </summary>
        public void Clear() {
            lock (_drawingsSYNC) {
                _grpdrawings.Clear();
            }
        }

        /// <summary>
        /// Element Count
        /// </summary>
        public int Count {
            get {
                lock (_drawingsSYNC) {
                    return _grpdrawings.Count;
                }
            }
        }

        /// <summary>
        /// Get an immutable snapshot of all Drawing Elements currently contained in this Group
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IDrawable> GetSnapshot() {
            lock (_drawingsSYNC) {
                return new List<IDrawable>(_grpdrawings);
            }
        }

        #endregion


        /// <summary>
        /// Draws all Elements to the given Gfx Context
        /// </summary>
        /// <param name="G"></param>
        public void Draw(System.Drawing.Graphics G) {
            foreach (var d in GetSnapshot()) {
                if (d != null) {
                    try {
                        d.Draw(G);
                    } catch (Exception e) {
                        //_drawingErrors.Add(e);
                    }
                }
            }
        }
    }
}
