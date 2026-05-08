#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using AIWars.Data;

namespace AIWars.EditorTools
{
    // One-click generator: reads Assets/GameData/llm_units_seed.csv and creates one
    // LLMUnitData ScriptableObject per row in Assets/GameData/LLMUnits/.
    public static class SeedLLMUnitsFromCsv
    {
        const string CsvPath = "Assets/GameData/llm_units_seed.csv";
        const string OutDir  = "Assets/GameData/LLMUnits";

        [MenuItem("AIWars/Seed LLM Unit Data From CSV")]
        public static void Run()
        {
            if (!File.Exists(CsvPath))
            {
                Debug.LogError($"[Seed] Missing {CsvPath}");
                return;
            }
            if (!AssetDatabase.IsValidFolder(OutDir))
            {
                Directory.CreateDirectory(OutDir);
                AssetDatabase.Refresh();
            }

            string[] lines = File.ReadAllLines(CsvPath);
            int created = 0;
            for (int i = 1; i < lines.Length; i++)
            {
                var cols = lines[i].Split(',');
                if (cols.Length < 8) continue;

                var asset = ScriptableObject.CreateInstance<LLMUnitData>();
                asset.modelId             = cols[0];
                asset.displayName         = cols[1];
                asset.vendorName          = cols[2];
                asset.faction             = cols[3] == "USA" ? Faction.USA :
                                            cols[3] == "China" ? Faction.China : Faction.Neutral;
                asset.tokensPerSecond     = float.Parse(cols[4]);
                asset.intelligenceScore   = float.Parse(cols[5]);
                asset.pricePerMillionTokens = float.Parse(cols[6]);
                asset.flavorText          = $"Role: {cols[7]}";

                // Trigger OnValidate to fill computed stats.
                asset.GetType().GetMethod("OnValidate",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                    ?.Invoke(asset, null);

                string path = $"{OutDir}/LLMUnit_{cols[0]}.asset";
                AssetDatabase.CreateAsset(asset, path);
                created++;
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[Seed] Created {created} LLMUnitData asset(s) in {OutDir}");
        }
    }
}
#endif
