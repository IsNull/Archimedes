using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;

namespace WPFCommon.Adorners
{
    /// <summary>
    /// Draws simple Circles around the bounding of the Control
    /// </summary>
    public class SimpleCircleAdorner : Adorner
    {
        SolidColorBrush renderBrush = new SolidColorBrush(Colors.Green);
        Pen renderPen = new Pen(new SolidColorBrush(Colors.Navy), 1.5);
        double renderRadius = 2.0;

        public SimpleCircleAdorner(UIElement adornedElement)
            : base(adornedElement) {
            renderBrush.Opacity = 0.2;
        }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext) {
            Rect adornedElementRect = new Rect(this.AdornedElement.DesiredSize);

            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopLeft, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.TopRight, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomLeft, renderRadius, renderRadius);
            drawingContext.DrawEllipse(renderBrush, renderPen, adornedElementRect.BottomRight, renderRadius, renderRadius);
        }
    }
}
