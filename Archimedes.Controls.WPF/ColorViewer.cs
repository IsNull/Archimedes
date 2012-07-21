using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace Archimedes.Controls.WPF
{
    public class ColorViewer : FrameworkElement
    {
        #region --- DependencyProperties ---

        public static readonly DependencyProperty SectorBrushesProperty =
          DependencyProperty.Register(
             "SectorBrushes",
             typeof(ObservableCollection<Brush>),
             typeof(ColorViewer),
             new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnSectorBrushesPropertyChanged)
          );

        /// <summary>
        /// The Brushes that draw the circle's sectors; The number of sectors is determined by
        /// the number of brushes contained in this list.
        /// </summary>
        public ObservableCollection<Brush> SectorBrushes {
            get { return (ObservableCollection<Brush>)GetValue(SectorBrushesProperty); }
            set { SetValue(SectorBrushesProperty, value); }
        }

        private static void OnSectorBrushesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var colorViewer = d as UIElement;
            if (colorViewer != null) {
                colorViewer.InvalidateVisual();
            }
        }


        public static readonly DependencyProperty StrokeColorProperty =
          DependencyProperty.Register(
             "StrokeColor",
             typeof(Color),
             typeof(ColorViewer),
             new UIPropertyMetadata(Colors.Transparent, OnStrokeColorPropertyChanged)
          );


        private static void OnStrokeColorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var colorViewer = d as UIElement;
            if (colorViewer != null) {
                colorViewer.InvalidateVisual();
            }
        }

        /// <summary>
        /// Sets/retrieves the color of the outer circle.
        /// This needs to be non-transparent if a line is to be drawn around the control (circle).
        /// </summary>
        public Color StrokeColor {
            get { return (Color)GetValue(StrokeColorProperty); }
            set { 
                SetValue(StrokeColorProperty, value);
                _strokePen = null;
                this.InvalidateVisual(); 
            }
        }

        public static readonly DependencyProperty StrokeWidthProperty =
          DependencyProperty.Register(
             "StrokeWidth",
             typeof(double),
             typeof(ColorViewer),
             new UIPropertyMetadata(1.0)
          );
        /// <summary>
        /// Sets/retrieves the width of the outer circle's stroke.
        /// This needs to be >0 if a line is to be drawn around the control (circle).
        /// </summary>
        public double StrokeWidth {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { 
                SetValue(StrokeWidthProperty, value);
                _strokePen = null;
                this.InvalidateVisual();
            }
        }

        #endregion

        #region Protected Properties

        private Pen _strokePen = null;

        protected Pen CachedStrokePen
        {
            get
            {
                if (_strokePen == null)
                {
                    _strokePen = new Pen(new SolidColorBrush(this.StrokeColor), this.StrokeWidth);
                }
                return _strokePen;
            }
        }

        #endregion

        #region --- C'tor ---

        /// <summary>
        /// C'tor
        /// </summary>
        public ColorViewer() {
            this.SectorBrushes = new ObservableCollection<Brush>();
            //this.CacheMode = new BitmapCache();
        }

        #endregion

        #region --- Handlers ---

        void SectorBrushes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            //If the collection of Brushes (and thus sectors) changed, we need to redraw the control.
            this.InvalidateVisual();
        }

        private bool useRenderCache = true;

        protected override void OnRender(System.Windows.Media.DrawingContext dc) {
            //We do all the rendering so skip the call to the default OnRender-method
            //base.OnRender(dc);


            if ((this.SectorBrushes == null || this.SectorBrushes.Count == 0))
                return;


            if (useRenderCache)
            {
                // if the given image is not present in this size, render in a offscreen bitmap and cache it.
                // otherwise the cached version is returned imedialty

                BitmapSource bs = CachedRenderColorChartToBitmap(this.RenderSize.Width, this.RenderSize.Height);
                dc.DrawImage(bs, new Rect(0, 0, bs.Width, bs.Height));
            }
            else
            {
                //render directly into the dc
                RenderColorChart(dc, (int)this.RenderSize.Width, (int)this.RenderSize.Height);
            }

           

        }

        #endregion


        #region Visual Render Cache

        private readonly IDictionary<int, BitmapSource> bmpCache = new Dictionary<int, BitmapSource>();

        BitmapSource CachedRenderColorChartToBitmap(double width, double height)
        {
            int hash = calcHash(width, height);

            if (!bmpCache.ContainsKey(hash))
            {
                bmpCache.Add(hash, RenderColorChartToBitmap(width, height));
            }

            return bmpCache[hash];
        }

        private int calcHash(double a, double b)
        {
            return ((int)a * 1000) + (int)b;
        }

        #endregion
        

        private BitmapSource RenderColorChartToBitmap(double width, double height)
        {
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                RenderColorChart(drawingContext, width, height);
                drawingContext.Close();
            }

            RenderTargetBitmap bitmap = new RenderTargetBitmap(
                (int)Math.Ceiling(width),
                (int)Math.Ceiling(height),
                96, // dpi x
                96, // dpi y
                PixelFormats.Pbgra32);
            bitmap.Render(drawingVisual);

            //
            // Note:
            //
            // The RenderTargetBitmap seems to hold some GDI handles alive.
            // In our usecase, we can have tousands of such bitmaps, and this leads to a leaking and finaly an exception of the underliing GDI Com interface.
            // To avoid this, we clone our Bitmap into a much simpler Writeable-Bitmap and release the RenderTargetBitmap
            //

            BitmapSource simpler = new WriteableBitmap(bitmap);
            simpler.Freeze(); // freeze it for best performance
            bitmap = null;
            drawingVisual = null;
            return simpler;
        }


        private void RenderColorChart(System.Windows.Media.DrawingContext dc, double width, double height)
        {
            //Determine the radius of the overall circle; we want this to be square in order to get a true circle, so use the min. extent
            double dblRadius = Math.Min(width / 2, height / 2);
            Size szRadius = new Size(dblRadius, dblRadius);

            //Calculate the center of the control
            Point ptCenter = new Point(dblRadius, dblRadius);


            if (this.SectorBrushes != null && this.SectorBrushes.Count > 1) {

                //The radius (degrees) that a single sector will cover
                double dblSectorRadius = 360d / this.SectorBrushes.Count;

                for (int intSectorCounter = 0; intSectorCounter < this.SectorBrushes.Count; intSectorCounter++) {
                    //Get the start- and end-points of the current arc segment to be drawn
                    Point ptArcStartPoint = getPointAtAngle(ptCenter, dblRadius, intSectorCounter * dblSectorRadius);
                    Point ptArcEndPoint = getPointAtAngle(ptCenter, dblRadius, (intSectorCounter + 1) * dblSectorRadius);

                    //The bounding rectangle of the current arc Sector
                    Rect rctArcRect = new Rect(ptArcStartPoint, ptArcEndPoint);

                    //Construct the shape
                    PathGeometry pg = new PathGeometry();
                    PathFigure pf = new PathFigure();
                    pg.Figures.Add(pf);
                    pf.StartPoint = ptArcStartPoint;
                    pf.IsFilled = true;

                    // Add the current sector's arc-segment
                    pf.Segments.Add(
                      new ArcSegment(
                         ptArcEndPoint,
                         szRadius,
                         dblSectorRadius,
                         (dblSectorRadius >= 180),
                         SweepDirection.Clockwise,
                         false
                       )
                      );

                    // Add a line that ends in the center of the control
                    pf.Segments.Add(
                       new LineSegment(ptCenter, true)
                      );

                    // Close the figure (IOW, this will add a line between the center of the control and 
                    // the arc's start point, resulting in a pie shape)
                    pf.IsClosed = true;

                    //Draw the arc (skipping the Pen used for drawing a line around the shape)
                    dc.DrawGeometry(this.SectorBrushes[intSectorCounter], null, pg);
                }
            } else if (this.SectorBrushes != null && this.SectorBrushes.Count == 1) {
                dc.DrawEllipse(this.SectorBrushes[0], null, ptCenter, dblRadius, dblRadius);
            }

            //Draw a circle around the sectors, if both a non-transparent color and a StrokeWidth>0 have been supplied.
            if (this.StrokeColor != null && this.StrokeColor != Colors.Transparent && this.StrokeWidth > 0)
                dc.DrawEllipse(null, this.CachedStrokePen, ptCenter, dblRadius, dblRadius);
        }


        #region --- Helpers ---

        /// <summary>
        /// Calculates the coordinates of a point on a circle, at a given angle.
        /// </summary>
        /// <param name="ptCenter">The center-coordinates of the circle</param>
        /// <param name="dblRadius">The radius of the circle</param>
        /// <param name="dblAtAngle">The angle at which to retrieve the coordinates</param>
        private Point getPointAtAngle(Point ptCenter, double dblRadius, double dblAtAngle) {
            double dblX = ptCenter.X + (dblRadius * Math.Cos(dblAtAngle / 180 * Math.PI));
            double dblY = ptCenter.Y + (dblRadius * Math.Sin(dblAtAngle / 180 * Math.PI));

            return new Point(dblX, dblY);
        }

        #endregion
    }
}

