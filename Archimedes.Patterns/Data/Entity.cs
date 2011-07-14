using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Data
{
    /// <summary>
    /// Entity which uses a int as PK
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityInt<T> : Entity<int, T> 
        where T : class
    {
            public EntityInt(){ }
            public EntityInt(int id): base(id){ }
    }

    /// <summary>
    /// Entity which uses a Guid as PK
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityGuid<T> : Entity<Guid, T>
        where T : class
    {
        public EntityGuid() { }
        public EntityGuid(Guid id) : base(id) { }
    }
    
    /// <summary>
    /// Baseclass for entities
    /// </summary>
    /// <typeparam name="Tid">ID type of this entity</typeparam>
    /// <typeparam name="T">Type of this entity</typeparam>
    public class Entity<Tid, T> : EntityBase<Tid, T>
        where Tid : struct
        where T : class
    {
        public Entity() : base() { }
        public Entity(Tid id) : base(id) {}
    }

    /// <summary>
    /// Baseclass for entities
    /// </summary>
    /// <typeparam name="Tid">ID type of this entity</typeparam>
    /// <typeparam name="T">Type of this entity</typeparam>
    public class EntityBase<Tid, T> where T : class
    {
        public EntityBase(){ }

        public EntityBase(Tid id) { ID = id; }

        /// <summary>
        /// ID of this Entity
        /// </summary>
        public virtual Tid ID {
            get;
            set;
        }

        #region IEquatable

        public virtual bool Equals(T obj) {
            var other = obj as EntityBase<Tid, T>;
            if (other == null)
                return false;
            return other.ID.Equals(this.ID);
        }

        public override bool Equals(object obj) {
            return this.Equals(obj as T);
        }

        public override int GetHashCode() {
            return this.ID.GetHashCode();
        }

        #endregion

        public override string ToString() {
            return this.ID.ToString();
        }
    }
}
