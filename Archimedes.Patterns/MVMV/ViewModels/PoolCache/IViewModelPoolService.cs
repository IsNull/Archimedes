using System;
namespace Archimedes.Patterns.MVMV.ViewModels.PoolCache
{
    public interface IViewModelPoolService
    {
        void Register(object domainmodelInstance, ICacheable viewModelInstance);
        void Register(ICacheable viewModelInstance);
        T Resolve<T>() where T : class;
        T Resolve<T>(object domainModel) where T : class;
    }
}
