using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns
{
    /// <summary>
    /// Generic EventData
    /// </summary>
    /// <typeparam name="T">Type of submited Value</typeparam>
    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value){
            Value = value;
        }

        /// <summary>
        /// Eventdata Value
        /// </summary>
        public T Value {
            get;
            protected set;
        }
    }
}
