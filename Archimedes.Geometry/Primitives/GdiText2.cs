/*******************************************
 * 
 *  Written by Pascal Büttiker (c)
 *  April 2010
 * 
 * *****************************************
 * *****************************************/
using System;
using System.Text;
using System.Drawing;
using Archimedes.Geometry.Extensions;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace Archimedes.Geometry.Primitives
{

    public enum TextAligning
    {
        Centered
    }
    /// <summary>
    /// Simple 2D Text, supports no rotation/orientation
    /// </summary>
    public class GdiText2 : IGeometryBase, IClosedGeometry
    {
        #region Private Data

        private string text = "";

        private PointF location = new PointF();
        private TextAligning textAligning = TextAligning.Centered;

        //private SizeF mTextSize = new SizeF();
        private RectangleF textSizeBounding = new RectangleF();

        private bool updateBoundingBox = false;
        private bool updateTextRect = false;

        private Font font = null;
       
        public int boundingBorder = 0;

        private Pen pen = Pens.Black;
        private Brush brush = new SolidBrush(Color.White);
        StringAlignment textHorizontalAlign = StringAlignment.Center;

        #endregion

        #region Constructors

        public GdiText2() { }

        public GdiText2(string uText) { this.Text = uText; }

        public GdiText2(string uText, Font uFont) 
            : this (uText){
            this.Font = uFont;
        }
        public GdiText2(string uText, Font uFont, PointF uTextMiddlePoint)
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
            get { return font; }
            set 
            { 
                font = value;
                updateBoundingBox = true;
                updateTextRect = true;
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
            get { return text; }
            set {
                text = value;
                updateTextRect = true;
                updateBoundingBox = true;
            }
        }

        public TextAligning Aligning {
            get { return textAligning; }
            set { textAligning = value; }
        }
        
        public StringAlignment TextHorizontalAlign {
            get { return textHorizontalAlign; }
            set { textHorizontalAlign = value; }
        }

        public Color BackGroundColor {
            get { 
            
                if(brush != null)
                    return (brush as SolidBrush).Color;
                return Color.White;
            }
            set { (brush as SolidBrush).Color = value; }
        }

        /// <summary>
        /// Defines the additional Border around the BoundingBox
        /// </summary>
        public int BoundingBorder {
            get { return this.boundingBorder; }
            set {
                if (this.boundingBorder == value) { return; }
                this.boundingBorder = value;
                updateBoundingBox = true;
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
        private Rectangle GetTextBoundRect() {
            SizeF tmpSize;
            Rectangle boundingBox;

            if (String.IsNullOrEmpty(Text) || Font == null) {
                return new Rectangle();
            }

            var bText = new SolidBrush(Color.Black);

            // find the approximation of the text size
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
                boundingBox = scanBMP.BoundingBox(Color.White);
            }

            if (this.Aligning == TextAligning.Centered) {
                boundingBox.X = (int)(this.Location.X - (boundingBox.Width / 2));
                boundingBox.Y = (int)(this.Location.Y - (boundingBox.Height / 2));
            } else
                throw new NotImplementedException();
            return boundingBox;
        }

        #endregion

        #region Geomerty Base

        public PointF Location {
            get { return location; }
            set { 
                location = value;
                updateBoundingBox = true;
            }
        }

        public void Move(Vector2 mov) {
            this.Location = mov.GetPoint(this.Location);
        }

        public void Scale(float fact) {
            throw new NotImplementedException();
        }

        public PointF MiddlePoint {
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
            get { return pen; }
            set { pen = value; }
        }

        public void Draw(Graphics G) {
            G.FillRectangle(new SolidBrush(this.BackGroundColor), this.BoundingBox);
            G.DrawString(this.Text, this.Font, this.Pen.Brush, this.BoundingBox, GetStringFormat());
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

        public bool IntersectsWith(IGeometryBase other) {
            if (other is GdiText2) 
                return this.BoundingBox.IntersectsWith(other.BoundingBox);
             else 
                return other.IntersectsWith(this);
        }

        public IEnumerable<PointF> Intersect(IGeometryBase other) {
            if (other is GdiText2) 
                return IntersectRect(other.BoundingBox);
            else
                return other.Intersect(this);
        }

        private IEnumerable<PointF> IntersectRect(RectangleF other) {
            var pnts = new List<PointF>();
            foreach (var side in Line2.RectExplode(this.BoundingBox)) {
                pnts.AddRange(side.InterceptRect(other));
            }
            return pnts;
        }


        public Circle2 BoundingCircle {
            get { return new Rectangle2(this.BoundingBox).BoundingCircle; }
        }

        public RectangleF BoundingBox {
            get {
                if (updateBoundingBox) {
                    var BoundingBox = GetTextBoundRect();
                    textSizeBounding = BoundingBox;
                    updateBoundingBox = false;
                }
                return textSizeBounding;
            }
        }

        public bool Contains(PointF uPoint) {
            return this.BoundingBox.Contains(new Point((int)uPoint.X, (int)uPoint.Y));
        }


        #endregion

        public IEnumerable<PointF> ToVertices() {
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

        public void Dispose() {
            this.Pen.Dispose();
            this.FillBrush.Dispose();
            this.Font.Dispose();
        }

        #region IClosedGeometry

        public float Area {
            get { return BoundingBox.Area(); }
        }

        public Brush FillBrush {
            get { return brush; }
            set { brush = value as SolidBrush; }
        }

        #endregion
    }
}
