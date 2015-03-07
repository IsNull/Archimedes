/*******************************************
 * 
 *  Written by Pascal Büttiker (c)
 *  April 2010
 * 
 * *****************************************
 * *****************************************/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Archimedes.Geometry.Primitives;

namespace Archimedes.Geometry.Rendering
{

    public enum TextAligning
    {
        Centered
    }
    /// <summary>
    /// Simple 2D Text, supports no rotation/orientation
    /// </summary>
    public class GdiText2 : IShape
    {
        #region Fields

        string _text = "";

        private Vector2 _location = new Vector2();
        private AARectangle _textSizeBounding = AARectangle.Empty;



        private TextAligning _textAligning = TextAligning.Centered;

        private bool _updateBoundingBox = false;
        private bool _updateTextRect = false;
        private Font _font = null;

        private int _boundingBorder = 0;

        private Pen _pen = Pens.Black;
        private Brush _brush = Brushes.White;
        private StringAlignment _textHorizontalAlign = StringAlignment.Center;

        #endregion

        #region Constructors

        public GdiText2() { }

        public GdiText2(string uText) { this.Text = uText; }

        public GdiText2(string uText, Font uFont) 
            : this (uText){
            this.Font = uFont;
        }
        public GdiText2(string uText, Font uFont, Vector2 uTextMiddlePoint)
            : this(uText, uFont) {
            this.MiddlePoint = uTextMiddlePoint;
        }

        public GdiText2(GdiText2 prototype) {
            Prototype(prototype);
        }

        public void Prototype(IGeometryBase iprototype) {
            var prototype = iprototype as GdiText2;
            if (prototype == null)
                throw new NotSupportedException();

                this.Text =prototype.Text;
                this.Font = prototype.Font;
                this.Pen = prototype.Pen;
                this.BackGroundColor = prototype.BackGroundColor;
                this.Location = prototype.Location;
                this.BoundingBorder = prototype.BoundingBorder;
                this.TextHorizontalAlign = prototype.TextHorizontalAlign;
        }

        #endregion

        #region Visualizer Propertys

        public Font Font
        {
            get { return _font; }
            set 
            { 
                _font = value;
                _updateBoundingBox = true;
                _updateTextRect = true;
            }
        }

        #endregion

        #region Public Propertys

        public bool IsEmpty {
            get {
                return this.Text == "";
            }
        }

        public string Text {
            get { return _text; }
            set {
                _text = value;
                _updateTextRect = true;
                _updateBoundingBox = true;
            }
        }

        public TextAligning Aligning {
            get { return _textAligning; }
            set { _textAligning = value; }
        }
        
        public StringAlignment TextHorizontalAlign {
            get { return _textHorizontalAlign; }
            set { _textHorizontalAlign = value; }
        }

        public Color BackGroundColor {
            get { 
            
                if(_brush != null)
                    return (_brush as SolidBrush).Color;
                return Color.White;
            }
            set { (_brush as SolidBrush).Color = value; }
        }

        /// <summary>
        /// Defines the additional Border around the BoundingBox
        /// </summary>
        public int BoundingBorder {
            get { return this._boundingBorder; }
            set {
                if (this._boundingBorder == value) { return; }
                this._boundingBorder = value;
                _updateBoundingBox = true;
            }
        }

        #endregion

        #region Private Helper Methods


        /// <summary>Calulate the Size a drawn Text consumes
        /// 
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="Font"></param>
        /// <returns></returns>
        /// 
        private AARectangle GetTextBoundRect() {
            SizeF tmpSize;
            AARectangle boundingBox;

            if (String.IsNullOrEmpty(Text) || Font == null)
            {
                return AARectangle.Empty;
            }

            var bText = new SolidBrush(Color.Black);

            // Find the approximation of the text size
            using (var Gtmp = Graphics.FromImage(new Bitmap(100, 100))) {
                tmpSize = Gtmp.MeasureString(Text, Font);
            }

            // now create a minimal bmp with approx size
            using (var scanBMP = new Bitmap((int)tmpSize.Width, (int)tmpSize.Height)) {
                using (var gScan = Graphics.FromImage(scanBMP)) {
                    gScan.Clear(Color.White);
                    gScan.DrawString(Text, Font, bText, 1, 1, GetStringFormat());
                    bText.Dispose();
                }
                boundingBox = BitmapUtil.BoundingBox(scanBMP, Color.White);
            }

            if (this.Aligning == TextAligning.Centered) {

                var pos = new Vector2(
                    this.Location.X - (boundingBox.Width / 2.0),
                    this.Location.Y - (boundingBox.Height / 2.0));
                boundingBox = new AARectangle(pos, boundingBox.Size);

            } else
                throw new NotImplementedException();
            return boundingBox;
        }

        #endregion

        #region Geomerty Base

        public Vector2 Location {
            get { return _location; }
            set { 
                _location = value;
                _updateBoundingBox = true;
            }
        }

        public void Translate(Vector2 mov) {
            this.Location += mov;
        }

        public void Scale(double fact) {
            throw new NotImplementedException();
        }

        public Vector2 MiddlePoint {
            get {
                if (this.Aligning == TextAligning.Centered) {
                    return Location;
                }
                throw new NotImplementedException("Only centered Text is implemented for now!");
            }
            set {
                if (this.Aligning == TextAligning.Centered) {
                    this.Location = value;
                    return;
                }
                throw new NotImplementedException("Only centered Text is implemented for now!");
            }
        }

        public IGeometryBase Clone() {
            return new GdiText2(this);
        }

        #endregion

        #region Geomerty Base Drawing

        public Pen Pen {
            get { return _pen; }
            set { _pen = value; }
        }

        public void Draw(Graphics g)
        {
            var rect = RectangleFUtil.ToRectangleF(BoundingBox);
            g.FillRectangle(new SolidBrush(this.BackGroundColor), rect);
            g.DrawString(this.Text, this.Font, this.Pen.Brush, rect, GetStringFormat());
        }

        private StringFormat GetStringFormat() {
            return new StringFormat(StringFormatFlags.NoClip | StringFormatFlags.NoWrap)
            { //StringFormatFlags.NoClip | StringFormatFlags.FitBlackBox)
                Alignment = this.TextHorizontalAlign,       // horizontal alignment
                LineAlignment = StringAlignment.Center,     // vertical alignment
                Trimming = StringTrimming.None
            };
        }

        public void AddToPath(GraphicsPath path) {
            throw new NotImplementedException("GdiText has not implemented AddToPath()!");
        }

        #endregion

        #region GeomertryBase Collision

        public bool IntersectsWith(IGeometryBase other, double tolerance = GeometrySettings.DEFAULT_TOLERANCE) {
            if (other is GdiText2)
                return this.BoundingBox.IntersectsWith(other.BoundingBox); // TODO handle tolerance!!
             else
                return other.IntersectsWith(this, tolerance);
        }

        public IEnumerable<Vector2> Intersect(IGeometryBase other, double tolerance = GeometrySettings.DEFAULT_TOLERANCE) {
            if (other is GdiText2)
                return IntersectRect(other.BoundingBox, tolerance);
            else
                return other.Intersect(this, tolerance);
        }

        private IEnumerable<Vector2> IntersectRect(AARectangle other, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {
            var pnts = new List<Vector2>();
            foreach (var side in Line2.RectExplode(this.BoundingBox)) {
                pnts.AddRange(side.InterceptRect(other, tolerance));
            }
            return pnts;
        }


        public Circle2 BoundingCircle {
            get { return new Rectangle2(this.BoundingBox).BoundingCircle; }
        }

        public AARectangle BoundingBox {
            get {
                if (_updateBoundingBox) {
                    var boundingBox = GetTextBoundRect();
                    _textSizeBounding = boundingBox;
                    _updateBoundingBox = false;
                }
                return _textSizeBounding;
            }
        }

        public bool Contains(Vector2 uPoint, double tolerance = GeometrySettings.DEFAULT_TOLERANCE)
        {
            // TODO handle tolerance
            return this.BoundingBox.Contains(new PointF((float)uPoint.X, (float)uPoint.Y));
        }

        #endregion

        public Vertices ToVertices() {
            return this.BoundingBox.ToVertices();
        }

        public Polygon2 ToPolygon2() {
            return new Polygon2(this.ToVertices())
            {
                Pen = this.Pen,
                FillBrush = this.FillBrush
            };
        }

        public override string ToString() {
            string str;

            str = "text: " + this.Text + "\n";
            str += "pos: " + this.Location.ToString() + "\n";
            str += "boundingbox: " + this.BoundingBox.ToString() + "\n";
            str += "font: " + this.Font.ToString() + "\n";
            return str;
        }

        #region IShape

        public double Area {
            get { return BoundingBox.Area; }
        }

        public Brush FillBrush {
            get { return _brush; }
            set { _brush = value; }
        }

        #endregion

        public void Dispose() {
            //
        }
    }
}
