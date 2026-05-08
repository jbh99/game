using UnityEngine;
using AIWars.Data;

namespace AIWars.Units
{
    [RequireComponent(typeof(UnitHealth))]
    [RequireComponent(typeof(UnitCombat))]
    [RequireComponent(typeof(UnitLocomotion))]
    [RequireComponent(typeof(ActionValueSystem))]
    public class Unit : MonoBehaviour
    {
        [SerializeField] LLMUnitData data;
        [SerializeField] UnitRuntimeSet runtimeSet;

        public LLMUnitData Data => data;
        public Faction Faction => data != null ? data.faction : Faction.Neutral;
        public bool IsPossessed { get; private set; }

        UnitHealth _health;
        UnitCombat _combat;
        UnitLocomotion _loco;
        ActionValueSystem _av;

        void Awake()
        {
            _health = GetComponent<UnitHealth>();
            _combat = GetComponent<UnitCombat>();
            _loco   = GetComponent<UnitLocomotion>();
            _av     = GetComponent<ActionValueSystem>();
        }

        void OnEnable()
        {
            if (runtimeSet != null) runtimeSet.Add(this);
            ApplyData();
        }

        void OnDisable()
        {
            if (runtimeSet != null) runtimeSet.Remove(this);
        }

        public void Configure(LLMUnitData newData)
        {
            data = newData;
            ApplyData();
        }

        void ApplyData()
        {
            if (data == null) return;
            _health.Initialize(data.baseHP, data.baseArmor);
            _combat.Initialize(data.baseDamage);
            _loco.Initialize(data.baseSpeed);
            _av.Initialize(data.actionValue);
        }

        public void SetPossessed(bool possessed)
        {
            IsPossessed = possessed;
            _loco.SetManualControl(possessed);
            _combat.SetManualControl(possessed);
            // 50% buff is applied by combat/locomotion when IsPossessed is true.
        }

        public float CurrentDamage    => _combat.GetCurrentDamage(IsPossessed);
        public float CurrentMoveSpeed => _loco.GetCurrentSpeed(IsPossessed);
    }
}
