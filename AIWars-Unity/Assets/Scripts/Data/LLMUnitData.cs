using UnityEngine;

namespace AIWars.Data
{
    public enum Faction { USA, China, Neutral }

    [CreateAssetMenu(menuName = "AIWars/LLM Unit Data", fileName = "LLMUnit_")]
    public class LLMUnitData : ScriptableObject
    {
        [Header("Identity")]
        public string modelId;
        public string displayName;
        public string vendorName;
        public Faction faction;
        [TextArea] public string flavorText;
        public Sprite portrait;
        public GameObject prefab;

        [Header("Source Benchmark Numbers")]
        [Tooltip("Output tokens per second — drives Action Value")]
        public float tokensPerSecond = 100f;
        [Tooltip("Composite intelligence score 0-100 — drives HP / damage / armor")]
        [Range(0, 100)] public float intelligenceScore = 50f;
        [Tooltip("USD per 1M output tokens — drives unit cost")]
        public float pricePerMillionTokens = 1f;

        [Header("Computed Stats (auto-derived; can be overridden)")]
        public bool overrideComputedStats = false;
        public float actionValue = 100f;
        public float baseDamage = 10f;
        public float baseHP = 100f;
        public float baseArmor = 5f;
        public float baseSpeed = 4f;
        public int unitCost = 100;

        public const float ACTION_VALUE_CONSTANT = 10000f;

        public float ComputedActionValue => ACTION_VALUE_CONSTANT / Mathf.Max(0.01f, tokensPerSecond);
        public float ComputedHP => 50f + intelligenceScore * 4f;
        public float ComputedDamage => 5f + intelligenceScore * 0.5f;
        public float ComputedArmor => intelligenceScore * 0.2f;
        public int ComputedCost => Mathf.Max(10, Mathf.RoundToInt(pricePerMillionTokens * 50f));

        void OnValidate()
        {
            if (!overrideComputedStats)
            {
                actionValue = ComputedActionValue;
                baseHP = ComputedHP;
                baseDamage = ComputedDamage;
                baseArmor = ComputedArmor;
                unitCost = ComputedCost;
            }
        }
    }
}
