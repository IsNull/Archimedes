using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Archimedes.Patterns.WPF.Behaviours
{
    public abstract class ControlBehaviour : IDisposable
    {
        private bool useBehaviour;
        private FrameworkElement atachedElement = null;

        public ControlBehaviour(FrameworkElement e) {
            AttachedElement = e;
        }

        public FrameworkElement AttachedElement {
            get { return atachedElement; }
            set {
                if (value == null){
                    DeAttach();
                    return;
                }
                if(atachedElement != null){
                    DeAttach();
                }
                Attach(value);
                atachedElement = value;
            }
        }

        /// <summary>
        /// Occurs when a new Control is attached to this behaviour
        /// </summary>
        /// <param name="e">Element to atach this behaviour</param>
        protected abstract void Attach(FrameworkElement e);

        /// <summary>
        /// Remove this Behaviour
        /// </summary>
        protected virtual void DeAttach() {
            atachedElement = null;
        }


        public bool IsAtached {
            get {
                return (this.AttachedElement != null);
            }
        }

        public bool UseBehaviour {
            set { useBehaviour = true; }
            get { return useBehaviour; }
        }

        public void Dispose() {
            DeAttach();
        }
    }
}
