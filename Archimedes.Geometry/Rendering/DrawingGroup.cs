using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Geometry.Rendering
{
    public class DrawingGroup : IDrawable, IEnumerable<IDrawable>
    {
        private List<IDrawable> grpdrawings = new List<IDrawable>();
        private List<Exception> drawingErrors = new List<Exception>();

        public  string Name = "";

        public DrawingGroup(string uname) {
            Name = uname;
        }


        public IDrawable this[int index] {
            get { return grpdrawings[index]; }
            set { grpdrawings[index] = value; }
        }

        public void Add(IDrawable d) {
            grpdrawings.Add(d);
        }
        public void AddRange(IEnumerable<IDrawable> d) {
            grpdrawings.AddRange(d);
        }
        public void Remove(IDrawable d) {
            grpdrawings.Remove(d);
        }
        public void Clear() {
            grpdrawings.Clear();
        }

        public IDrawable[] ToArray() {
            return grpdrawings.ToArray();
        }


        public int Count {
            get { return grpdrawings.Count; }
        }

        public void Draw(System.Drawing.Graphics G) {
            foreach (var d in grpdrawings) {
                if (d != null) {
                    try {
                        d.Draw(G);
                    } catch (Exception e) {
                        drawingErrors.Add(e);
                    }
                }
            }
        }

        public IEnumerator<IDrawable> GetEnumerator() {
            return grpdrawings.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return (grpdrawings as System.Collections.IEnumerable).GetEnumerator();
        }
    }
}
