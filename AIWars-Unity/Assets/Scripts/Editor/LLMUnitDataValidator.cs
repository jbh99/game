#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using AIWars.Data;

namespace AIWars.EditorTools
{
    // Build pre-process check: every LLMUnitData asset must have valid stat ranges
    // and must reference a prefab. Catches missing data before it reaches play mode.
    public class LLMUnitDataValidator : UnityEditor.Build.IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
        {
            string[] guids = AssetDatabase.FindAssets("t:LLMUnitData");
            int errors = 0;
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var d = AssetDatabase.LoadAssetAtPath<LLMUnitData>(path);
                if (d == null) continue;
                if (d.tokensPerSecond <= 0)        { Debug.LogError($"[LLM] Invalid tokensPerSecond on {path}"); errors++; }
                if (d.pricePerMillionTokens < 0)   { Debug.LogError($"[LLM] Invalid price on {path}"); errors++; }
                if (d.intelligenceScore < 0 || d.intelligenceScore > 100)
                                                   { Debug.LogError($"[LLM] intelligence out of range on {path}"); errors++; }
                if (d.prefab == null)              { Debug.LogWarning($"[LLM] Missing prefab on {path}"); }
            }
            if (errors > 0)
                throw new UnityEditor.Build.BuildFailedException($"{errors} LLMUnitData asset(s) failed validation.");
            Debug.Log("[LLM] LLMUnitData validation passed.");
        }
    }

    public static class AIWarsMenu
    {
        [MenuItem("AIWars/Validate LLM Unit Data")]
        public static void ValidateNow()
        {
            new LLMUnitDataValidator().OnPreprocessBuild(null);
        }

        [MenuItem("AIWars/Open Localization Folder")]
        public static void OpenLocFolder()
        {
            EditorUtility.RevealInFinder("Assets/Resources/Localization/ko.json");
        }
    }
}
#endif
