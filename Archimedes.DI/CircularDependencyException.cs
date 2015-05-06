using System;
using System.Runtime.CompilerServices;

namespace Archimedes.DI
{
    public class CircularDependencyException : Exception
    {

        public CircularDependencyException(Type a, Type b) : this(a.Name + " depends on " + b.Name + " and vice versa (circular dependency). Fix that!")
        {
            
        }

        public CircularDependencyException(string message) : base(message)
        {
            
        }
    }
}
