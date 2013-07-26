using System;

namespace Archimedes.Maps
{
    /// <summary>
    /// Represents a Location defined by an Adress
    /// </summary>
    [Serializable]
    public class Location : ICloneable, IEquatable<Location>, IComparable<Location>, IComparable
    {
        #region Fields

        string _title = null;
        string _country;
        string _street;
        int _plz;
        string _place;

        #endregion

        #region Events

        public virtual event EventHandler TitleChanged;

        public virtual event EventHandler PlaceChanged;

        public virtual event EventHandler PLZChanged;

        public virtual event EventHandler StreetChanged;

        public virtual event EventHandler CountryChanged;

        #endregion

        #region Constructor

        public Location() { }

        public Location(Location prototype) {
            Prototype(prototype);
        }

        #endregion
        
        #region Public Properties

        public virtual int ID {
            get;
            set;
        }


        public virtual string Title {
            get {
                if(PLZ == 0)
                    return String.Format("{0}, {1}", Place, Street);
                else
                    return String.Format("{0} {1}, {2}", Place, PLZ, Street);
            }
            set {
                _title = value;
            }
        }

        public virtual string Place {
            get { return _place; }
            set { 
                _place = value;
                if (PlaceChanged != null)
                    PlaceChanged(this, EventArgs.Empty);
                OnTitleChanged();
            }
        }

        public virtual int PLZ {
            get { return _plz; }
            set { 
                _plz = value;
                if (PLZChanged != null)
                    PLZChanged(this, EventArgs.Empty);
                OnTitleChanged();
            }
        }

        public virtual string Street {
            get { return _street; }
            set { 
                _street = value;
                if (StreetChanged != null)
                    StreetChanged(this, EventArgs.Empty);
                OnTitleChanged();
            }
        }


        public virtual string Country {
            get { return _country; }
            set { 
                _country = value;
                if (CountryChanged != null)
                    CountryChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Public Methods

        protected virtual void Prototype(Location prototype) {
            this.Title = prototype.Title;
            this.Place = prototype.Place;
            this.PLZ = prototype.PLZ;
            this.Street = prototype.Street;
            this.Country = prototype.Country;
        }

        public virtual object Clone() {
            return new Location(this);
        }

        public override string ToString()
        {
            return Street + ", " + Place + ", " + Country;
        }

        public virtual string ToFormatedAddress()
        {
            string addr = "";
            var nl = Environment.NewLine;
            addr = Title + nl + Street + nl + Country;
            return addr;
        }


        #endregion

        #region Event Handlers

        void OnTitleChanged() {
            if (TitleChanged != null)
                TitleChanged(this, EventArgs.Empty);
        }

        #endregion

        #region IEquality

        public virtual bool Equals(Location other) {
            if (this == other)
                return true;
            else {
                return ((other.Place == this.Place) &&
                    (other.Street == this.Street) &&
                    (other.PLZ == this.PLZ) &&
                    (other.Country == this.Country));
            }
        }

        public override int GetHashCode() {
            return (this.Title + this.Street + this.Place + this.Country).GetHashCode();
        }


        public override bool Equals(object obj) {
            var other = obj as Location;
            if (other != null) {
                return Equals(other);
            }else
                return false;
        }

        #endregion

        #region IComparable

        public virtual int CompareTo(Location other) {

            if (this == other)
                return 0;
            if (other == null)
                return -1;

            if (this.PLZ == other.PLZ)
                return 0;

            return (this.PLZ > other.PLZ) ? 1 : -1;
        }

        public virtual int CompareTo(object obj) {
            var other = obj as Location;
            if (other != null)
                return CompareTo(other);
            else
                return -1;
        }

        #endregion
    }
}
