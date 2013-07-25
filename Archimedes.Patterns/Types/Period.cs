using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Globalization;

namespace Archimedes.Patterns.Types
{
    [DataContract]
    public enum PeriodFormat
    {
        [EnumMember] MonthsAndYears,
        [EnumMember] Months,
        [EnumMember] Years
    }

    /// <summary>
    /// Represents a TimePeriod
    /// </summary>
    [DataContract]
    public struct Period
    {
        const int DaysPerMonth = 30;

        [DataMember]
        long _hours;

        public Period(long hours) {
            _hours = hours;
        }

        #region Static Creators

        public static Period FromYears(long years) {
            return new Period(years * 24 * DaysPerMonth * 12);
        }

        public static Period FromMonths(long months) {
            return new Period(months * 24 * DaysPerMonth);
        }

        public static Period FromDays(long days) {
            return new Period(days * 24);
        }

        static Period _zero = new Period(0);
        public static Period Zero {
            get { return _zero; }
        }

        #endregion

        #region Properties

        public long Hours {
            get { return _hours; }
            //set { _hours = value; }
        }

        public long Days {
            get { return _hours / 24; }
        }

        public long Months {
            get { return _hours / (24 * DaysPerMonth); }
        }

        public long Years {
            get { return _hours / (24 * DaysPerMonth * 12); }
        }

        public bool IsEmpty {
            get { return _hours == 0; }
        }

        #endregion

        #region Public Methods

        public Period Abs() {
            return new Period(Math.Abs(this.Hours));
        }

        public Period RemoveAbs(Period abs) {
            if (this < Period.Zero) {
                return (this.Abs() - abs) * -1;
            } else {
                return this - abs;
            }
        }

        #endregion

        #region Operators

        #region Period Operators

        public static Period operator +(Period right, Period left) {
            return new Period(right.Hours + left.Hours);
        }

        public static Period operator -(Period right, Period left) {
            return new Period(right.Hours - left.Hours);
        }

        public static bool operator <(Period right, Period left) {
            return right.Hours < left.Hours;
        }

        public static bool operator >(Period right, Period left) {
            return right.Hours > left.Hours;
        }

        public static bool operator <=(Period right, Period left) {
            return right.Hours <= left.Hours;
        }

        public static bool operator >=(Period right, Period left) {
            return right.Hours >= left.Hours;
        }

        public static bool operator ==(Period right, Period left) {
            return right.Hours == left.Hours;
        }

        public static bool operator !=(Period right, Period left) {
            return right.Hours != left.Hours;
        }

        #endregion

        #region Scalar Operators

        public static Period operator *(Period right, int left) {
            return new Period(right.Hours * left);
        }

        public static Period operator *(int right, Period left) {
            return left * right;
        }

        public static Period operator /(Period right, int left) {
            return new Period(right.Hours / left);
        }

        

        #endregion

        #region DateTime Operators

        public static DateTime operator +(DateTime date, Period period) {
            int years = (int)period.Years;
            if (years > 0) {
                date = date.AddYears(years);
                period = period.RemoveAbs(Period.FromYears(years));
            }

            int months = (int)period.Months;
            if (months > 0) {
                date = date.AddMonths(months);
                period = period.RemoveAbs(Period.FromMonths(months));
            }

            if (!period.IsEmpty)
                date = date.AddHours(period.Hours);

            return date;
        }

        public static DateTime operator +(Period right, DateTime left) {
            return left + right;
        }

        
        public static DateTime operator -(DateTime date, Period period) {
            int years = (int)period.Years;
            if (years > 0) {
                date = date.AddYears(-years);
                period = period.RemoveAbs(Period.FromYears(years));
            }

            int months = (int)period.Months;
            if (months > 0) {
                date = date.AddMonths(-months);
                period = period.RemoveAbs(Period.FromMonths(months));
            }

            if (!period.IsEmpty)
                date = date.AddHours(-period.Hours);

            return date;
        }
        
        public static DateTime operator -(Period right, DateTime left) {
            return left - right;
        }

        #endregion

        #endregion


        public bool Equals(Period period) {
            return period.Hours == this.Hours;
        }

        public override bool Equals(object obj) {
            if (obj is Period)
                return Equals((Period)obj);
            else
                return false;
        }

        public override int GetHashCode() {
            return this.Hours.GetHashCode();
        }

        public string ToString(string format, CultureInfo culture) {
            return this.ToString();
        }

        public string ToString(PeriodFormat pformat) {
            switch (pformat) {

                case PeriodFormat.Months:
                return this.Months.ToString(CultureInfo.CurrentCulture);

                case PeriodFormat.Years:
                return this.Years.ToString(CultureInfo.CurrentCulture);

                default:
                return this.ToString();
            }
        }

        public override string ToString() {
            var sb = new StringBuilder(); 

            var per = this;

            if (per.Years > 0) {
                sb.Append(per.Years + " Years");
                per = per.RemoveAbs(Period.FromYears((per.Years)));
            }

            if (this.Months > 0) {
                if (sb.Length > 0)
                    sb.Append(", ");
                sb.Append(this.Months + " Months");
                per = per.RemoveAbs(Period.FromMonths((per.Months)));
            }

            return sb.ToString();
        }
    }
}
