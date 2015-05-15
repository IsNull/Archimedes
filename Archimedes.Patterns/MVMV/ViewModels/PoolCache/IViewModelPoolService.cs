using System;
namespace Archimedes.Patterns.MVMV.ViewModels.PoolCache
{
    /// <summary>
    /// Pool cache for often used ViewModels
    /// </summary>
    public interface IViewModelPoolService
    {
        void Register(object domainmodelInstance, ICacheable viewModelInstance);
        void Register(ICacheable viewModelInstance);
        T Resolve<T>() where T : class,ICacheable;
        T Resolve<T>(object domainModel) where T : class,ICacheable;
    }
}
