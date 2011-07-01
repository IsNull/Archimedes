using System;
namespace Archimedes.Patterns.Conditions
{
    /// <summary>
    /// Provides low level type - Operator mapping services
    /// This Service is commonly used Application wide
    /// </summary>
    public interface IOperatorManagerService
    {
        /// <summary>
        /// Return all possible Operators for the given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        System.Collections.Generic.IEnumerable<Operator> GetPossibleOperatorsForType(Type type);

        /// <summary>
        /// Register Operators for the given type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="possibleOperators"></param>
        void RegisterOperatorsForType(Type type, System.Collections.Generic.IEnumerable<Operator> possibleOperators);

        /// <summary>
        /// Returns a string representation for the given OP Flag(s)
        /// </summary>
        /// <param name="opFlags"></param>
        /// <returns></returns>
        string OperatorToString(Operator opFlags);
    }
}
