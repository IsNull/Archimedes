using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Maps.Services
{
    public interface IGeoProjectionService
    {
        /// <summary>
        /// Gets the distance between two coordinates
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        double GetDistance(GeoCoordinate a, GeoCoordinate b);
    }
}
