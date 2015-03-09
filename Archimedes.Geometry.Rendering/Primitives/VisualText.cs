using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Archimedes.Geometry.Primitives;

namespace Archimedes.Geometry.Rendering.Primitives
{
    public class VisualText : Visual
    {
        #region Fields

        private readonly Rectangle2 _rectangle = Rectangle2.Empty;

        private string _text;
        private Font _font;
        private StringAlignment _textHorizontalAlign;

        private SizeD? _textSizeCache;


        #endregion

        /// <summary>
        /// Creates a new visual text at the given location
        /// </summary>
        /// <param name="location"></param>
        /// <param name="text"></param>
        /// <param name="font"></param>
        public VisualText(Vector2 location, string text, Font font = null)
        {
            Location = location;
            _text = text;
            _font = font ?? SystemFonts.DefaultFont;
        }

        #region Properties

        public Vector2 Location
        {
            get { return _rectangle.Location; }
            set
            {
                _rectangle.Location = value;
                Invalidate();
            }
        }

        public Vector2 MiddlePoint
        {
            get { return _rectangle.MiddlePoint; }
            set
            {
                _rectangle.MiddlePoint = value;
                Invalidate();
            }
        }



        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                Invalidate();
            }
        }

        public Font Font
        {
            get { return _font; }
            set
            {
                _font = value;
                Invalidate();
            }
        }
        
        public StringAlignment TextHorizontalAlign
        {
            get { return _textHorizontalAlign; }
            set
            {
                _textHorizontalAlign = value;
                Invalidate();
            }
        }

        public TextAligning Aligning { get; set; }


        public override IGeometryBase Geometry
        {
            get
            {
                return _rectangle;
            }
        }

        protected void Prototype(VisualText other)
        {
            base.Prototype(other);

            this.Text = other.Text;
            this.Font = other.Font;
            this.TextHorizontalAlign = other.TextHorizontalAlign;
            this.Aligning = other.Aligning;
        }

        #endregion


        #region Drawing

        public override void Draw(Graphics g)
        {
            var rect = RectangleFUtil.ToRectangleF(GetTextRenderRect());
            if (FillBrush != null)
            {
                g.FillRectangle(FillBrush, rect);
            }
            g.DrawString(this.Text, this.Font, this.Pen.Brush, rect, GetStringFormat());
        }

        private StringFormat GetStringFormat()
        {
            return new StringFormat(StringFormatFlags.NoClip | StringFormatFlags.NoWrap)
            { //StringFormatFlags.NoClip | StringFormatFlags.FitBlackBox)
                Alignment = this.TextHorizontalAlign,       // horizontal alignment
                LineAlignment = StringAlignment.Center,     // vertical alignment
                Trimming = StringTrimming.None
            };
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get the rect where the text will be drawn into
        /// </summary>
        /// <returns></returns>
        /// 
        private AARectangle GetTextRenderRect()
        {
            AARectangle renderRect;

            var textSize = TextSize;

            switch (Aligning)
            {
                case TextAligning.Centered:
                     var pos = new Vector2(
                    _rectangle.Location.X - (textSize.Width / 2.0),
                    _rectangle.Location.Y - (textSize.Height / 2.0)
                    );
                renderRect = new AARectangle(pos, textSize);
                    break;

                default:
                    throw new NotSupportedException("Aligning of type " + Aligning + " not supported!");
            }
 
            return renderRect;
        }



        /// <summary>
        /// Invalidates all cached valuess
        /// </summary>
        private void Invalidate()
        {
            _textSizeCache = null;
        }

        /// <summary>
        /// Gets the exacte size this text with current font and settings will take
        /// </summary>
        private SizeD TextSize
        {
            get
            {
                if (!_textSizeCache.HasValue)
                {
                    _textSizeCache = CalcTextSize(Text, Font, GetStringFormat());
                }
                return _textSizeCache.Value;
            }
        }


        /// <summary>
        /// Calculates the text size
        /// </summary>
        /// <returns></returns>
        private static SizeD CalcTextSize(string text, Font font, StringFormat format)
        {
            SizeF tmpSize;
            AARectangle boundingBox;

            if (String.IsNullOrEmpty(text) || font == null)
            {
                return SizeD.Empty;
            }

            var bText = new SolidBrush(Color.Black);

            // Find the approximation of the text size
            using (var g = Graphics.FromImage(new Bitmap(100, 100)))
            {
                tmpSize = g.MeasureString(text, font);
            }

            // now create a minimal bmp with approx size
            using (var scanBMP = new Bitmap((int)tmpSize.Width, (int)tmpSize.Height))
            {
                using (var gScan = Graphics.FromImage(scanBMP))
                {
                    gScan.Clear(Color.White);
                    gScan.DrawString(text, font, bText, 1, 1, format);
                    bText.Dispose();
                }
                boundingBox = BitmapUtil.BoundingBox(scanBMP, Color.White);
            }
            return boundingBox.Size;
        }



        public override Visual Clone()
        {
            var copy = new VisualText(_rectangle.Location, _text);
            copy.Prototype(this);
            return copy;
        }

        #endregion
    }
}
