using System;
using UnityEngine;

namespace AIWars.Data
{
    [CreateAssetMenu(menuName = "AIWars/Variables/Resource Pool", fileName = "Resources_")]
    public class PlayerResourcePool : ScriptableObject
    {
        [SerializeField] int credits = 1000;
        public int Credits => credits;
        public event Action<int> OnChanged;

        public bool TrySpend(int amount)
        {
            if (credits < amount) return false;
            credits -= amount;
            OnChanged?.Invoke(credits);
            return true;
        }

        public void Add(int amount)
        {
            credits += amount;
            OnChanged?.Invoke(credits);
        }

        public void SetTo(int amount)
        {
            credits = amount;
            OnChanged?.Invoke(credits);
        }
    }
}
