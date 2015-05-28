using System;
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
        private readonly IConfigurationService _configurationService;

        #endregion


        public ValueConfigurator(IConfigurationService configurationService)
        {
            this._configurationService = configurationService;
        }

        public void SetValue(FieldInfo field, object instance, string valueExpression)
        {
            var value = InterpretValueExpression(valueExpression);
            SetValueAutoConvert(field, instance, value);
        }

        private string InterpretValueExpression(string expression)
        {
            return _variableReference.Replace(expression, match =>
            {
                var variable = match.Groups[1].Value;

                var valueOpt = _configurationService.GetOptional(variable);

                if (!valueOpt.IsPresent)
                {
                    Log.Warn("Could not find configuration key '" + variable + "'!");
                }

                return valueOpt.OrDefault();
            });
        }


        private void SetValueAutoConvert(FieldInfo field, object instance, string value)
        {
            if (field.FieldType == typeof (string))
            {
                field.SetValue(instance, value);
            }
            else
            {
                throw new NotSupportedException("The value configurator does not support converting a string to your '" + field.FieldType + "' type!");
            }
            // TODO Support more primitives

        }
    }
}
