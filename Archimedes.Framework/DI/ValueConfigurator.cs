using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using Archimedes.Framework.Configuration;
using log4net;

namespace Archimedes.Framework.DI
{
    internal class ValueConfigurator
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Regex _variableReference = new Regex(@"\$\{(.*)\}");
        private readonly Properties _configuration;

        #endregion


        public ValueConfigurator(Properties configuration)
        {
            _configuration = configuration;
        }

        public void SetValue(FieldInfo field, object instance, string valueExpression)
        {
            try
            {
                var value = InterpretValueExpression(valueExpression);
                SetValueAutoConvert(field, instance, value);
            }
            catch (Exception e)
            {
                throw new ValueConfigurationException("Failed to set value " + valueExpression + " to field " + field + " at class " + instance.GetType(), e);
            }
        }

        private string InterpretValueExpression(string expression)
        {
            return _variableReference.Replace(expression, match =>
            {
                var variable = match.Groups[1].Value;

                var valueOpt = _configuration.GetOptional(variable);

                if (!valueOpt.IsPresent)
                {
                    Log.Warn("Could not find configuration key '" + variable + "'!");
                }

                return valueOpt.OrDefault();
            });
        }


        private void SetValueAutoConvert(FieldInfo field, object instance, string value)
        {
            if (field.FieldType == typeof(string))
            {
                field.SetValue(instance, value);
            }
            else
            {
                try
                {
                    var typeConverter = TypeDescriptor.GetConverter(field.FieldType);
                    object propValue = typeConverter.ConvertFromString(null, CultureInfo.InvariantCulture, value);
                    field.SetValue(instance, propValue);
                }
                catch (NotSupportedException e)
                {
                    throw new NotSupportedException("The value configurator does not support converting a string '" + value + "' to your '" + field.FieldType + "' type!", e);
                }
            }
        }
    }
}
