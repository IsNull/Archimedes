using System;
namespace Archimedes.Patterns.Deployment
{
    public interface IVersionStore
    {
        Version GetVersionFor(string id);
        void SetVersionFor(string id, Version version);
    }
}
