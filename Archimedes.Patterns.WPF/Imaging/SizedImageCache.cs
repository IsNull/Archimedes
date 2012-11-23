using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Archimedes.Patterns.WPF.Imaging
{
    public class SizedImageCache<T> : Archimedes.Patterns.WPF.Imaging.ISizedImageCache<T>
        where T : ImageSource
    {
        private readonly Dictionary<object, Dictionary<Size, T>> _imageCache 
            = new Dictionary<object, Dictionary<Size,T>>();

        /// <summary>
        /// Store the given image in this cache
        /// </summary>
        /// <param name="imageID"></param>
        /// <param name="imgsrc"></param>
        public void Store(object imageID, Size size, T imgsrc)
        {
            lock (_imageCache)
            {
                //var size = new Size((int)imgsrc.Width, (int)imgsrc.Height);

                if (!_imageCache.ContainsKey(imageID))
                {
                    _imageCache.Add(imageID, new Dictionary<Size, T>());
                }

                if (_imageCache[imageID].ContainsKey(size))
                {
                    _imageCache[imageID][size] = imgsrc;
                }
                else
                {
                    _imageCache[imageID].Add(size, imgsrc);
                }
               
            }
        }



        public T Retrive(object imageID, Size size)
        {
            lock (_imageCache)
            {
                T img = null;
                if (_imageCache.ContainsKey(imageID) && _imageCache[imageID].ContainsKey(size))
                {
                    img = _imageCache[imageID][size];
                }
                return img;
            }
        }

        public void Clear()
        {
            lock (_imageCache)
            {
                _imageCache.Clear();
            }
        }

        public bool Contains(object imageID, Size size)
        {
            lock (_imageCache)
            {
                return (_imageCache.ContainsKey(imageID) && _imageCache[imageID].ContainsKey(size));
            }
        }


    }
}
