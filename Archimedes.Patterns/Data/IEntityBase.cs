using System;
namespace Archimedes.Patterns.Data
{
    public interface IEntityBase<Tid, T>
     where T : class
    {
        bool Equals(object obj);
        bool Equals(T obj);
        int GetHashCode();

        /// <summary>
        /// ID of this Entity
        /// </summary>
        Tid Id { get; set; }
        string ToString();
    }
}
