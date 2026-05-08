using System.Collections.Generic;
using UnityEngine;

namespace AIWars.Data
{
    public abstract class RuntimeSet<T> : ScriptableObject
    {
        public readonly List<T> Items = new();
        public void Clear() => Items.Clear();
        public void Add(T item) { if (!Items.Contains(item)) Items.Add(item); }
        public void Remove(T item) { if (Items.Contains(item)) Items.Remove(item); }
    }
}
