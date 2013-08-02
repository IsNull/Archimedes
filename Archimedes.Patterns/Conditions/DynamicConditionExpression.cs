﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Reflection;
using Archimedes.Patterns.Reflection;
using System.Linq.Expressions;

namespace Archimedes.Patterns.Conditions
{
    /// <summary>
    /// Represents a dynamic property conditon expression
    /// There are hardcoded greater/smallerthan emiters,
    /// which basicaly can handle numeric types / dates
    /// </summary>
    [DataContract]
    public class DynamicConditionExpression : ConditionExpression
    {
        #region Fields

        [NonSerialized]
        static readonly Type Number = typeof(double);

        [DataMember]
        string _propertyName;

        [DataMember]
        Operator _compOP = Operator.None;

        [DataMember]
        object _value;
        
        // working fields
        [NonSerialized]
        Type _itemType;
        [NonSerialized]
        PropertyInfo _property = null;
        [NonSerialized]
        bool _isNumeric;

        #endregion

        #region Events

        /// <summary>
        /// Raised when the Property Value has changed
        /// </summary>
        public virtual event EventHandler ValueChanged;

        /// <summary>
        /// Raised when the Property Operator has changed
        /// </summary>
        public virtual event EventHandler OperatorChanged;

        /// <summary>
        /// Raised when the Property "Proeperty" has changed
        /// </summary>
        public virtual event EventHandler PropertyChanged;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an new empty DynamicConditionExpression
        /// </summary>
        public DynamicConditionExpression() : base() { }


        /// <summary>
        /// Creates an new empty DynamicConditionExpression
        /// </summary>
        public DynamicConditionExpression(Guid newid) : base(newid) { }

        
        #endregion

        #region Public Properties

        /// <summary>
        /// Propertyname as string
        /// If just the string name is given, the Propertyinfo is garthered with reflection
        /// </summary>
        public virtual string PropertyName {
            get { return Property == null ? _propertyName : Property.Name; }
            set { 
                _propertyName = value;
                Property = null;
            }
        }

        /// <summary>
        /// Specifies a Property of a Class to use for comparision
        /// </summary>
        public virtual PropertyInfo Property {
            get {
                return _property;
            }
            set { 
                _property = value;

                if (value != null) {
                    _itemType = _property.PropertyType;
                    _isNumeric = TypeHelper.IsNumeric(_itemType);
                } else {
                    _itemType = null;
                    _isNumeric = false;
                }

                if (PropertyChanged != null)
                    PropertyChanged(this, EventArgs.Empty);

                this.Value = Value; // ensure that the value is still valid & compatible with the new property
            }
        }

        /// <summary>
        /// Operator to use for comparision
        /// </summary>
        public virtual Operator Operator {
            get { return _compOP; }
            set {
                _compOP = value;
                if (OperatorChanged != null)
                    OperatorChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Value to compare against
        /// </summary>
        public virtual object Value {
            get { return _value; }
            set { 
                if (_itemType != null)
                    _value = TypeHelper.ConvertOrDefault(value, _itemType, false);
                else
                    _value = value;
                if (ValueChanged != null)
                    ValueChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Extends the where clausle of the given IQueryable List with this condition filter
        /// </summary>
        /// <param name="dataQry"></param>
        /// <returns></returns>
        public override IQueryable<object> Filter(IQueryable<object> dataQry) {
            if (dataQry.Any()) {
                //
                // Cache Property Info if necessary
                //
                if (Property == null) {
                    Property =
                    dataQry.First().GetType().GetProperty(_propertyName);
                    if (_property == null)
                        throw new NotSupportedException(string.Format(
                            "Your objects in the provided List doesn't have the specified public property {0}!", _propertyName));
                }
            }
            return base.Filter(dataQry);
        }

        
        /// <summary>
        /// Checks it given Object Matches this dynamic condition
        /// </summary>
        /// <param name="obj">Object which has a matching Property</param>
        /// <returns></returns>
        public override bool IsMatch(object obj) {
            bool res = false;

            if (_property == null)
                throw new NotSupportedException(
                    string.Format("This call is not valid in the current state. Ensure that {0} is set",
                    "_property"));

            var val = _property.GetValue(obj, null);


            // Check here if the Expression is true
            // NOT will be handled in the Finalize Part of this Method and reverse
            // any result if NOT is set.

            //
            // Compare Equality if the Equals Flag is set
            // Do it also if NOT is the ONLY flag which is set, for convience.
            //
            if (((_compOP & Operator.Equal) == Operator.Equal)
                || _compOP == Operator.Not) {

                if ((val == null && _value == null) ||
                    (val != null && val.Equals(_value))) {
                    res = true;
                    goto Finalize;
                }
            }

            if ((_compOP & Operator.GreaterThan) == Operator.GreaterThan) {
                if (_isNumeric) {
                    res = (double)Convert.ChangeType(val, Number) > (double)Convert.ChangeType(_value, Number);
                } else if (val != null && TypeHelper.IsTypeOrUnderlingType(_itemType, typeof(DateTime))) {
                    res = (DateTime)val > (DateTime)_value;
                }
            }

            if ((_compOP & Operator.SmallerThan) == Operator.SmallerThan) {
                if (_isNumeric) {
                    res = (double)Convert.ChangeType(val, Number) < (double)Convert.ChangeType(_value, Number);
                } else if (val != null && TypeHelper.IsTypeOrUnderlingType(_itemType, typeof(DateTime))) {
                    res = (DateTime)val < (DateTime)_value;
                }
            }

        Finalize:
            return ((_compOP & Operator.Not) == Operator.Not) ? !res : res;
        }

        #endregion

        [NonSerialized]
        private const string _name = "Dynamische Bedingung";

        public override string Name {
            get { return _name; }
        }
    }



}
