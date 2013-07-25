using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Maps
{
    public class GeoRectRegion : GeoPolygon
    {
        readonly GeoCoordinate[] _coordinates = new GeoCoordinate[4];

        public GeoRectRegion()
        {
        }


        /// <summary>
        /// Name of the region
        /// </summary>
        public string Name { get; set; }



        public GeoCoordinate Bottom {
            get { return _coordinates[0]; }
            protected set { _coordinates[0] = value; }
        }

        public GeoCoordinate Left
        {
            get { return _coordinates[1]; }
            protected set { _coordinates[1] = value; }
        }

        public GeoCoordinate Right
        {
            get { return _coordinates[2]; }
            protected set { _coordinates[2] = value; }
        }

        public GeoCoordinate Top
        {
            get { return _coordinates[3]; }
            protected set { _coordinates[3] = value; }
        }


        public override int Count
        {
            get { return _coordinates.Length; }
        }


        /// <summary>
        /// Gets the Coordinates of this GeoRectRegion
        /// </summary>
        public override IEnumerable<GeoCoordinate> GetCoordinates()
        {
            return new List<GeoCoordinate>(_coordinates);
        }


    }
}
