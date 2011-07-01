using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archimedes.Patterns.Reflection;

namespace Archimedes.Patterns.Conditions
{
    public class OperatorManagerService : Archimedes.Patterns.Conditions.IOperatorManagerService
    {
        static Operator[] _numericOperators = new Operator[]
        {
            Operator.Equal, Operator.Not | Operator.Equal,
            Operator.GreaterThan, Operator.Equal | Operator.GreaterThan,
            Operator.SmallerThan, Operator.Equal | Operator.SmallerThan
        };

        Dictionary<Type, Operator[]> _opmap = new Dictionary<Type, Operator[]>()
        {
            { typeof(bool), new Operator[] { Operator.Equal, Operator.Not | Operator.Equal}}
        };


        public OperatorManagerService() {
            //
            // Add some hardcoded primitives to the operator mapping
            // Numbertypes, DateTime
            //
            // This list can be enlarged by user code with the RegisterOperatorsForType(t, operators) Method.
            //
            List<Type> _fullcompareablePrimitiveTypes = new List<Type>(TypeHelper.GetAllNumericTypes());
            _fullcompareablePrimitiveTypes.Add(typeof(DateTime));

            foreach (var t in _fullcompareablePrimitiveTypes) {
                RegisterOperatorsForType(t, _numericOperators);
            }
        }

        public IEnumerable<Operator> GetPossibleOperatorsForType(Type type) {

            if (TypeHelper.IsNullableType(type)) {
                type = Nullable.GetUnderlyingType(type);
            }
          

            if (_opmap.ContainsKey(type))
                return new List<Operator>(_opmap[type]);
            else
                return new List<Operator>();
        }

        public void RegisterOperatorsForType(Type type, IEnumerable<Operator> possibleOperators) {
            if (_opmap.ContainsKey(type))
                _opmap[type] = possibleOperators.ToArray();
            else
                _opmap.Add(type, possibleOperators.ToArray());
        }
        
        /// <summary>
        /// Returns a string Representation 
        /// </summary>
        /// <param name="opFlags"></param>
        /// <returns></returns>
        public virtual string OperatorToString(Operator opFlags) {
            StringBuilder sb = new StringBuilder();

            if ((opFlags & Operator.Not) == Operator.Not)
                sb.Append("!");

            if ((opFlags & Operator.SmallerThan) == Operator.SmallerThan)
                sb.Append('<');
            
            if ((opFlags & Operator.GreaterThan) == Operator.GreaterThan)
                sb.Append('>');

            if ((opFlags & Operator.Equal) == Operator.Equal)
                sb.Append('=');

            return sb.ToString();
        }

    }
}
