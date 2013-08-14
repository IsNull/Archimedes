using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Archimedes.Controls.WPF
{
    /// <summary>
    /// Thread safe shared backbuffer cache.
    ///
    /// As long as no memory shortcuts are present, the items will very likley stay in the cache,
    /// and items which are hit more often in the cache will stay longer alive.
    /// 
    /// </summary>
    internal class SharedBackbufferCache
    {
        private readonly Dictionary<int, Dictionary<int, BitmapSource>> _cache = new Dictionary<int, Dictionary<int, BitmapSource>>();
        private readonly object _cacheLock = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sizeHash"></param>
        /// <param name="contentHash"></param>
        /// <returns></returns>
        public BitmapSource GetBackbuffer(int sizeHash, int contentHash)
        {
            BitmapSource source = null;

            lock (_cacheLock)
            {
                if (_cache.ContainsKey(sizeHash) && _cache[sizeHash].ContainsKey(contentHash))
                {
                    source = _cache[sizeHash][contentHash];
                }
            }
            return source;
        }


        public void UpdateBackbuffer(int sizeHash, int contentHash, BitmapSource buffer)
        {
            lock (_cacheLock)
            {
                if (!_cache.ContainsKey(sizeHash))
                {
                    _cache.Add(sizeHash, new Dictionary<int, BitmapSource>());
                }

                var innerDict = _cache[sizeHash];

                if (!innerDict.ContainsKey(contentHash))
                {
                    innerDict.Add(contentHash, null);
                }
                innerDict[contentHash] = buffer;
            }
        }
    }
}
