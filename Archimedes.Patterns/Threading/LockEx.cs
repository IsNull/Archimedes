using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Archimedes.Patterns.Threading
{
    public class LockEx : IDisposable
    {
        private readonly object[] _padlocks;
        private readonly bool[] _securedFlags;

        private LockEx(object padlock, int milliSecondTimeout) {
            _padlocks = new[] { padlock };
            _securedFlags = new[] { Monitor.TryEnter(padlock, milliSecondTimeout) };
        }

        private LockEx(object[] padlocks, int milliSecondTimeout) {
            _padlocks = padlocks;
            _securedFlags = new bool[_padlocks.Length];
            for(int i = 0; i < _padlocks.Length; i++)
                _securedFlags[i] = Monitor.TryEnter(padlocks[i], milliSecondTimeout);
        }

        public bool Secured {
            get { return _securedFlags.All(s => s); }
        }

        public static void Lock(object[] padlocks, int millisecondTimeout, Action codeToRun) {
            using(var bolt = new LockEx(padlocks, millisecondTimeout))
                if(bolt.Secured)
                    codeToRun();
                else
                    throw new TimeoutException(string.Format("LockEx.Lock wasn't able to acquire a lock in {0}ms",
                                                             millisecondTimeout));
        }

        public static void Lock(object padlock, int millisecondTimeout, Action codeToRun) {
            using(var bolt = new LockEx(padlock, millisecondTimeout))
                if(bolt.Secured)
                    codeToRun();
                else
                    throw new TimeoutException(string.Format("LockEx.Lock wasn't able to acquire a lock in {0}ms",
                                                             millisecondTimeout));
        }

        #region IDisposable

        public void Dispose() {
            for(int i = 0; i < _securedFlags.Length; i++)
                if(_securedFlags[i]) {
                    Monitor.Exit(_padlocks[i]);
                    _securedFlags[i] = false;
                }
        }

        #endregion
    }
}
