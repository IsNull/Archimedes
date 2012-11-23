using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Archimedes.Patterns.WPF.Imaging
{
    public class CachedImageResizer
    {
        private readonly ISizedImageCache<BitmapSource> cache = new SizedImageCache<BitmapSource>();

        /// <summary>
        /// Resizes the image when not already in the cache. otherwise, just returns the cached image
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public BitmapSource CachedResize(BitmapSource src, int width, int height)
        {
            BitmapSource resized = null;

            var s = new Size(width, height);

            if (cache.Contains(src, s))
            {
                resized = cache.Retrive(src, s);
            }else{
                resized = ImageSupport.CreateResizedImage(src, width, height);
                cache.Store(src, s, resized);
            }

            return resized;
        }

    }
}
