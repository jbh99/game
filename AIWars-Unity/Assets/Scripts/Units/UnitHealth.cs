using System;
using UnityEngine;

namespace AIWars.Units
{
    public class UnitHealth : MonoBehaviour
    {
        [SerializeField] float maxHP;
        [SerializeField] float armor;
        public float CurrentHP { get; private set; }
        public event Action<float, float> OnChanged;
        public event Action OnDeath;

        public void Initialize(float hp, float arm)
        {
            maxHP = hp;
            armor = arm;
            CurrentHP = maxHP;
            OnChanged?.Invoke(CurrentHP, maxHP);
        }

        public void TakeDamage(float rawDamage, bool armorPiercing = false)
        {
            float effective = armorPiercing ? rawDamage : Mathf.Max(1, rawDamage - armor);
            CurrentHP -= effective;
            OnChanged?.Invoke(CurrentHP, maxHP);
            if (CurrentHP <= 0f)
            {
                CurrentHP = 0f;
                OnDeath?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}
