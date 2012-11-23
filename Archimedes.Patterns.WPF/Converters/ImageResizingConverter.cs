using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media.Imaging;
using Archimedes.Patterns.WPF.Imaging;
using System.Windows.Controls;

namespace Archimedes.Patterns.WPF.Converters
{
    public class ImageResizingConverter : MarkupExtension, IMultiValueConverter
    {
        private readonly static CachedImageResizer _cachedimageResizer = new CachedImageResizer();

        public Image ImageControl { get; set; }
        public BitmapSource SourceImg { get; set; }
        public double DecodeWidth { get; set; }
        public double DecodeHeight { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            this.SourceImg = values[0] as BitmapImage;
            this.ImageControl = values[1] as Image;
            try
            {
                double ydelta = 0;
                double xdelta = 0;
                
                if (ImageControl != null)
                {
                    var ma = ImageControl.Margin;

                    ydelta = ma.Bottom;
                    xdelta = ma.Right;
                    //ydelta = (ma.Bottom + ma.Top) / 2;
                    //xdelta = (ma.Left + ma.Right) / 2;
                }

                this.DecodeWidth = (double)values[2] - xdelta;
                this.DecodeHeight = (double)values[3] - ydelta;
            }
            catch { }

            return DecodeImage(SourceImg, this.DecodeWidth, this.DecodeHeight);
        }

        private BitmapSource DecodeImage(BitmapSource src, double width, double height)
        {
            return _cachedimageResizer.CachedResize(src, (int)width, (int)height); ;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
