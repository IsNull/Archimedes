using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace Archimedes.Patterns.WPF.Imaging
{
    class ImageSupport
    {


        /// <summary>
        /// Converts the Frame to a Bitmapimage
        /// </summary>
        /// <param name="bitmapFrame"></param>
        /// <returns></returns>
        public static BitmapImage FrameToBitmap(BitmapFrame bitmapFrame)
        {
            BitmapImage bImg = new BitmapImage();
            BitmapEncoder encoder = new PngBitmapEncoder(); //new JpegBitmapEncoder();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                encoder.Frames.Add(bitmapFrame);
                encoder.Save(memoryStream);
                bImg.BeginInit();
                bImg.CacheOption = BitmapCacheOption.OnLoad;
                bImg.CreateOptions = BitmapCreateOptions.None;
                bImg.StreamSource = new MemoryStream(memoryStream.ToArray());
                bImg.EndInit();

                bImg.Freeze();
            }

            return bImg;
        }



        /// <summary>
        /// Resizes the given Image
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static BitmapImage ResizeImage(BitmapImage source, int width, int height)
        {
            var target = new TransformedBitmap(
                            source,
                            new ScaleTransform(
                                width / source.Width * 96 / source.DpiX,
                                height / source.Height * 96 / source.DpiY,
                                0, 0));

            var resizedFrame = BitmapFrame.Create(target);

            return FrameToBitmap(resizedFrame);
        }

        /// <summary>
        /// Creates a new ImageSource with the specified width/height
        /// </summary>
        /// <param name="source">Source image to resize</param>
        /// <param name="width">Width of resized image</param>
        /// <param name="height">Height of resized image</param>
        /// <returns>Resized image</returns>
        public static BitmapSource CreateResizedImage(ImageSource source, int width, int height)
        {
            // Target Rect for the resize operation
            Rect rect = new Rect(0, 0, width, height);

            // Create a DrawingVisual/Context to render with
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawImage(source, rect);
            }

            // Use RenderTargetBitmap to resize the original image
            RenderTargetBitmap resizedImage = new RenderTargetBitmap(
                (int)rect.Width, (int)rect.Height,  // Resized dimensions
                96, 96,                             // Default DPI values
                PixelFormats.Default);              // Default pixel format
            resizedImage.Render(drawingVisual);

            resizedImage.Freeze();

            // Return the resized image
            return resizedImage;
        }


    }
}
