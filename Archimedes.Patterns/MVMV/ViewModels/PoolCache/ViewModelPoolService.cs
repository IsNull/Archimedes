

using Archimedes.Patterns.Container;

namespace Archimedes.Patterns.MVMV.ViewModels.PoolCache
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics;

    /// <summary>
    /// Pool for common used ViewModels
    /// </summary>
    [Service]
    public class ViewModelPoolService : IViewModelPoolService
    {
        Dictionary<Type, ICacheable> _viewModels = new Dictionary<Type, ICacheable>();
        Dictionary<object, ICacheable> _domainModel2VMmap = new Dictionary<object, ICacheable>();


        /// <summary>
        /// Resolve ViewModel Instance by Interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>() where T : class {
            if (_viewModels.ContainsKey(typeof(T))) {
                return _viewModels[typeof(T)] as T;
            } else {
                foreach (var vt in _viewModels.Keys)
                    if (typeof(T).IsAssignableFrom(vt))
                        return _viewModels[vt] as T;
                Debug.Fail(string.Format("Can't find an Instance of {0} in the VM Pool!", typeof(T).Name));
                return null;
            }
        }

        /// <summary>
        /// Resolve ViewModel Instance by DomainModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>(object domainModel) where T : class {
            if (_domainModel2VMmap.ContainsKey(domainModel)) {
                return _domainModel2VMmap[domainModel] as T;
            } else
                return null;
        }

        /// <summary>
        /// Register a ViewModel Instance
        /// </summary>
        /// <param name="viewModelInstance"></param>
        public void Register(ICacheable viewModelInstance) {
            Type t = viewModelInstance.GetType();
            if (_viewModels.ContainsKey(t)) {
                _viewModels[t].CacheExpired -= OnCacheExpired;
                _viewModels[t] = viewModelInstance;
            } else {
                _viewModels.Add(viewModelInstance.GetType(), viewModelInstance);
            }
            viewModelInstance.CacheExpired += OnCacheExpired;
        }

        /// <summary>
        /// Register a ViewModel Instance with a matching domainmodel instance
        /// </summary>
        /// <param name="viewModelInstance"></param>
        public void Register(object domainmodelInstance, ICacheable viewModelInstance) {

            if(_domainModel2VMmap.ContainsKey(domainmodelInstance)){
                _domainModel2VMmap[domainmodelInstance].CacheExpired -= OnCacheExpired;
                _domainModel2VMmap[domainmodelInstance] = viewModelInstance;
            } else {
                _domainModel2VMmap.Add(domainmodelInstance, viewModelInstance);
            }
            viewModelInstance.CacheExpired += OnCacheExpired;
        }


        void OnCacheExpired(object sender, EventArgs e) {
            // remove the object from cache to allow disposing / GC Garbage collection
            // otherwise we produce a memory leak here

            #region Remove from Domain Model - VM Map

            var val = (from kv in _domainModel2VMmap
                       where ReferenceEquals(kv.Value, sender)
                       select kv.Key).ToList();
            foreach (var dm in val)
                _domainModel2VMmap.Remove(val.First());

            #endregion

            #region Remove from VM Cache

            Type t = sender.GetType();
            if (_viewModels.ContainsKey(t)) {
                if (ReferenceEquals(_viewModels[t], sender))
                    _viewModels[t] = null;
            }

            #endregion
        }

    }
}
