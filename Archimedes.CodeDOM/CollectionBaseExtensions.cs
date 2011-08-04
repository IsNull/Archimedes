using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Archimedes.CodeDOM
{
    public static class CollectionBaseExtensions
    {
        public static void ForEach<T>(this CollectionBase collection, Action<T> action) {
            foreach(var item in collection)
                action((T)item);
        }

        public static T Find<T>(this CollectionBase collection, Func<T, bool> match) {
            foreach(var item in collection)
                if(match((T)item))
                    return (T)item;
            return default(T);
        }

        public static IEnumerable<T> FindAll<T>(this CollectionBase collection, Func<T, bool> match) {
            foreach(var item in collection)
                if(match((T)item))
                    yield return (T)item;
        }
    }
}
