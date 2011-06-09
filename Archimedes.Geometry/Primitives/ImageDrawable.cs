using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Archimedes.Geometry.Extensions;

namespace Archimedes.Geometry.Primitives
{
    /// <summary>
    /// Wrapper Class Image <---> IGeometryBase
    /// </summary>
    public class ImageDrawable : Rectangle2, IDisposable
    {
        #region Fields

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
            get { return _image; }
            set {
                if (value == null) {
                    Size = new SizeF();
                } else {
                    try {
                        Size = value.Size;
                        _image = value;
                    } catch {
                        Size = new SizeF();
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

        public override void Draw(Graphics G) {
            if (_image != null)
                G.DrawImage(_image, new RectangleF(Location, Size));
        }

        #endregion
    }
}
