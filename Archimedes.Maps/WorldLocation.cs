using System;
using System.Runtime.Serialization;
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

        [DataMember]
        private GeoCoordinate _position = GeoCoordinate.Zero;

        [DataMember]
        private LocationAccuracy _accuracy = LocationAccuracy.Unknown;


        [NonSerialized]
        bool _positionResolveMade = false;
        [NonSerialized]
        readonly IGeoProjectionService _projectionService = ServiceLocator.Instance.Resolve<IGeoProjectionService>();
        [NonSerialized]
        readonly IGeoCodingService _geoCodingService = ServiceLocator.Instance.Resolve<IGeoCodingService>();

        #endregion

        #region Events

        /// <summary>
        /// Raised when the Position has changed
        /// </summary>
        public virtual event EventHandler PositionChanged;

        /// <summary>
        /// Raised when the Accuracy has changed
        /// </summary>
        public virtual event EventHandler AccuracyChanged;

        

        #endregion

        #region Constructor

        public WorldLocation() { } 

        public WorldLocation(WorldLocation location) { 
            this.Prototype(location); 
        }

        public WorldLocation(Location location)
            : this(location, GeoCoordinate.Zero)
        {
        }

        public WorldLocation(Location location, GeoCoordinate coordinate)
        {
            _position = coordinate;
            base.Prototype(location);
        }

        #endregion

        #region From Methods

        public static WorldLocation WorldLocationFrom(GeoCoordinate point)
        {
            var geoCodingService = ServiceLocator.Instance.Resolve<IGeoCodingService>();
            Location location;
            geoCodingService.GeoCoderReverseResolve(point, out location);

            return new WorldLocation(location);
        }

        #endregion

        #region Properties


        public virtual void SetPosition(GeoCoordinate pos, LocationAccuracy accuracy)
        {
            Position = pos;
            Accuracy = accuracy;
        }


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
            protected set
            {
                if (_position != value) {
                    _position = value;
                    OnPositionChanged();
                }
            }
        }

        /// <summary>
        /// Gets the accuracy of the resolved position
        /// </summary>
        public virtual LocationAccuracy Accuracy
        {
            get { return _accuracy; }
            protected set
            {
                if (_accuracy != value)
                {
                    _accuracy = value;
                    OnAccuracyChanged();
                }
            }
        }


        /// <summary>
        /// Returns true if the current Adress is resolved into Geo Coords
        /// </summary>
        public virtual bool IsPositionResolved {
            get { return !_position.IsZero; }
        }

        #endregion

        #region Public Methods


        public virtual void Prototype(WorldLocation location) {
            base.Prototype(location);
            this.SetPosition(location.Position, location.Accuracy);
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
                stat = QueryPosition(force);
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

            var position = GeoCoordinate.Zero;
            var accuracy = LocationAccuracy.Unknown;
            bool res = false;

            if (_geoCodingService != null)
            {
                bool fromCache;
                
                res = _geoCodingService.GeoCoderResolve(this, false, out position, out status, out fromCache, out accuracy);
                _positionResolveMade = true;
            }

            SetPosition((res) ? position : GeoCoordinate.Zero, accuracy);

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

        protected virtual void OnAccuracyChanged() {
            if (AccuracyChanged != null)
                AccuracyChanged(this, null);
        }



        public override object Clone() {
            return new WorldLocation(this);
        }

        public override string ToString() {
            return this.Title;
        }
    }
}
