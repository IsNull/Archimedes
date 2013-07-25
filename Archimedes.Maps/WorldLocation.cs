using System;
using System.Threading.Tasks;
using Archimedes.Maps.GeoCoding;
using Archimedes.Maps.Services;
using Archimedes.Patterns.Services;

namespace Archimedes.Maps
{
    /// <summary>
    /// Represents a location (Adress) on the World which can be resolved into lat/lng Coordinates
    /// </summary>
    [Serializable]
    public class WorldLocation : Location, IEquatable<WorldLocation>, IComparable<WorldLocation>
    {
        #region Fields

        GeoCoordinate _position = GeoCoordinate.Empty;
        bool _positionResolveMade = false;

        readonly IGeoProjectionService _projectionService = ServiceLocator.Instance.Resolve<IGeoProjectionService>();
        readonly IGeoCodingService _geoCodingService = ServiceLocator.Instance.Resolve<IGeoCodingService>();

        #endregion

        #region Events

        /// <summary>
        /// Raised when the Position has changed
        /// </summary>
        public virtual event EventHandler PositionChanged;

        #endregion

        #region Constructor

        public WorldLocation() { } 

        public WorldLocation(WorldLocation location) { 
            this.Prototype(location); 
        }

        public WorldLocation(Location location){
            base.Prototype(location);
        }

        #endregion

        #region From Methods

        public static WorldLocation WorldLocationFrom(GeoCoordinate point)
        {
            var location = new WorldLocation();

            // todo resolve adress infromation from point

            throw new NotImplementedException();

            //return location;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the position in World Coordinates
        /// </summary>
        public virtual GeoCoordinate Position
        {
            get {
                if (!_positionResolveMade && !IsPositionResolved) {
                    QueryPosition();
                }
                return _position;
            }
            set {
                if (_position != value) {
                    _position = value;
                    OnPositionChanged();
                }
            }
        }

        /// <summary>
        /// Returns true if the current Adress is resolved into Geo Coords
        /// </summary>
        public virtual bool IsPositionResolved {
            get { return (!_position.IsEmpty); }
        }

        #endregion

        #region Public Methods

        public virtual void Prototype(WorldLocation location) {
            base.Prototype(location);
            if (location.IsPositionResolved)
                this.Position = location.Position;
        }

        /// <summary>
        /// Get the distance in direct air line to another Destination
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual decimal GetAirDistanceTo(WorldLocation other) {
            decimal distance = 0;

            if (this.IsPositionResolved && other.IsPositionResolved) {
                distance = (decimal)_projectionService.GetDistance(this.Position, other.Position);
            }
            return distance;
        }

        #endregion

        #region Position Query Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="force"></param>
        /// <returns></returns>
        public virtual async Task<GeoCodeStatus> QueryPositionAsync(bool force = false)
        {
            var stat = GeoCodeStatus.None;
            await Task.Run(() =>
            {
                stat = QueryPosition(true);
            });
            return stat;
        }


        /// <summary>
        /// Query the GeoCoder to resolve the location
        /// </summary>
        /// <param name="force">If the Position already was resolved, force to resolve it again?</param>
        /// <returns>Returns the status of the geocoder </returns>
        public virtual GeoCodeStatus QueryPosition(bool force = false)
        {
            var status = GeoCodeStatus.None;

            if (!force && IsPositionResolved)
                return GeoCodeStatus.Success; ;

            var position = GeoCoordinate.Empty;
            bool res = false;

            if (_geoCodingService != null)
            {
                res = _geoCodingService.GeoCoderResolve(this, out position, out status);
                _positionResolveMade = true;
            }

            Position = (res) ? position : GeoCoordinate.Empty;

            return status;
        }


        #endregion

        #region IEquatable

        public virtual bool Equals(WorldLocation other) {
            return base.Equals(other);
        } 

        #endregion

        #region IComparable

        public virtual int CompareTo(WorldLocation other) {
            return base.CompareTo(other as Location);
        }

        #endregion

        protected virtual void OnPositionChanged() {
            if (PositionChanged != null)
                PositionChanged(this, null);
        }

        public override object Clone() {
            return new WorldLocation(this);
        }

        public override string ToString() {
            return this.Title;
        }
    }
}
