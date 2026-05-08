using UnityEngine;

namespace AIWars.Data
{
    [CreateAssetMenu(menuName = "AIWars/Faction Data", fileName = "Faction_")]
    public class FactionData : ScriptableObject
    {
        public Faction faction;
        public string displayName;
        public Color factionColor = Color.white;
        public Sprite factionIcon;

        [Header("Home Region Buff")]
        [Tooltip("Region this faction gets a home advantage in")]
        public string homeRegionId;

        [Header("Home buff multipliers (1.0 = no change)")]
        public float productionSpeedMul = 1.20f;
        public float damageMul = 1.40f;
        public float costMul = 1.0f;

        public LLMUnitData[] roster;
    }
}
