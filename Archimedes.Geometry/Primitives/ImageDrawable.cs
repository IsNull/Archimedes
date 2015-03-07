using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Archimedes.Geometry.Extensions;

namespace Archimedes.Geometry.Primitives
{
    // TODO maybe refactor this for a general texture system...
    /// <summary>
    /// Wrapper Class Image <---> IGeometryBase
    /// </summary>
    public class ImageDrawable : Rectangle2
    {
        #region Fields

        private readonly object _syncLOCK = new object();
        private Image _image;
        private Vector2 _location;

        #endregion

        #region Constructors

        public ImageDrawable() {
            _location = new Vector2(0, 0);
        }

        public ImageDrawable(Image image, Vector2? location = null) : this() {
            Image = image;
            _location = location ?? _location;
        }

        #endregion

        #region Properties

        public Image Image {
            get {
                lock (_syncLOCK) {
                    return _image;
                }
            }
            set {
                lock (_syncLOCK) {
                    if (value == null)
                    {
                        Size = SizeD.Empty;
                    } else {
                        try {
                            Size = new SizeD(value.Size.Width, value.Size.Height);
                            _image = value;
                        } catch {
                            Size = SizeD.Empty;
                        }
                    }
                }
            }
        }

        public new Vector2 Location {
            get { return _location; }
            set {_location = value;}
        }

        #endregion

        #region Operator Casts

        public static explicit operator Image(ImageDrawable drawImg) {
            return drawImg.Image;
        }
        public static explicit operator ImageDrawable(Image image) {
            return new ImageDrawable(image);
        }

        #endregion

        #region IDisposable

        public override void Dispose() {
            if (_image != null)
                _image.Dispose();
            base.Dispose();
        }

        #endregion

        #region IDrawable

        public override void Draw(Graphics g) {
            lock (_syncLOCK) {
                if (_image != null)
                    g.DrawImage(_image, new RectangleF(Location, new SizeF((float)Size.Width, (float)Size.Height)));
            }
        }

        #endregion
    }
}
