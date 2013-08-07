using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Maps
{

    /// <summary>
    /// Represents the accuracy of a location
    /// </summary>
    public enum LocationAccuracy
    {
        /// <summary>
        /// Location accuracy is not supported
        /// </summary>
        NotSupported = -1,

        /// <summary>
        /// The accuracy is unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Indicates that the returned result is approximate.
        /// </summary>
        Approximate = 2,

        /// <summary>
        /// Indicates that the returned result is the geometric center of a result
        /// such as a polyline (for example, a street) or polygon (region).
        /// </summary>
        GeometricCenter = 3,


        /// <summary>
        /// Indicates that the returned result reflects an approximation (usually on
        /// a road) interpolated between two precise points (such as intersections).
        /// Interpolated results are generally returned when rooftop geocodes are
        /// unavailable for a street address.
        /// </summary>
        RangeInterpolated = 7,


        /// <summary>
        /// Indicates that the returned result is a precise geocode for which we have
        /// location information accurate down to street address precision.
        /// </summary>
        Rooftop = 9,
    }

    public static class AccuracyUtil
    {
        /// <summary>
        /// Turns an accuracy value (float {0.0 to 1.0}) into a LocationAccuracy
        /// </summary>
        /// <param name="value">Expected value range is: {-1, 0, 0.1, ...,1.0}</param>
        /// <returns></returns>
        public static LocationAccuracy ToLocationAccuracy(float value)
        {
           if(value <= 0)
           {
               return LocationAccuracy.Unknown;
           }

           if (value <= 0.3)
           {
               return LocationAccuracy.Approximate;
           }

           if (value <= 0.6)
           {
               return LocationAccuracy.GeometricCenter;
           }

           if (value <= 0.9)
           {
               return LocationAccuracy.RangeInterpolated;
           }

           return LocationAccuracy.Rooftop;
        }
    }
}
