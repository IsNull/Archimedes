using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns
{

    /// <summary>
    /// generic for singletons
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> where T : new()
    {
        // ctor
        [DebuggerStepThrough]
        protected Singleton() {
            if (Instance != null) {
                throw (new Exception("You have tried to create a new singleton class where you should have instanced it. " +
                                     String.Format("Replace your \"new {0}()\" with \"{0}.Instance\"", this.GetType().Name)));
            }
        }

        public static T Instance {
            get {
                return SingletonCreator.instance;
            }
        }

        class SingletonCreator
        {
            static SingletonCreator() {

            }
            internal static readonly T instance = new T();
        }
    }
}
