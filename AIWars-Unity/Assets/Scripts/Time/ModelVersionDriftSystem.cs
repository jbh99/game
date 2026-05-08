using System.Collections.Generic;
using UnityEngine;
using AIWars.Units;

namespace AIWars.TimeSystems
{
    // Implements the plan's time-based version drift events:
    //  - Claude Opus 4.6 forced update -> Hallucination Debuff (random target picks)
    //  - GPT-5.4 throttling event       -> action gauge decay slows
    public class ModelVersionDriftSystem : MonoBehaviour
    {
        public static ModelVersionDriftSystem Instance { get; private set; }

        public bool ClaudeHallucinating { get; private set; }
        public bool Gpt54Throttled { get; private set; }

        public float hallucinationDuration = 60f;
        public float throttleDuration = 45f;
        public float eventInterval = 180f;

        readonly HashSet<string> hallucinationModels = new() { "claude-opus-4-6" };
        readonly HashSet<string> throttledModels    = new() { "gpt-5-4" };

        float _timer;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= eventInterval)
            {
                _timer = 0f;
                StartCoroutine(RandomDriftEvent());
            }
        }

        System.Collections.IEnumerator RandomDriftEvent()
        {
            if (Random.value < 0.5f)
            {
                ClaudeHallucinating = true;
                Debug.Log("[Drift] Claude 4.6 hallucination event ON");
                yield return new WaitForSeconds(hallucinationDuration);
                ClaudeHallucinating = false;
                Debug.Log("[Drift] Claude hallucination event OFF");
            }
            else
            {
                Gpt54Throttled = true;
                Debug.Log("[Drift] GPT-5.4 throttling event ON");
                yield return new WaitForSeconds(throttleDuration);
                Gpt54Throttled = false;
                Debug.Log("[Drift] GPT-5.4 throttling event OFF");
            }
        }

        public bool ShouldHallucinate(Unit u) =>
            ClaudeHallucinating && u != null && u.Data != null && hallucinationModels.Contains(u.Data.modelId);

        public float GetActionTickMultiplier(Unit u)
        {
            if (Gpt54Throttled && u != null && u.Data != null && throttledModels.Contains(u.Data.modelId))
                return 0.4f; // slows action gauge tick
            return 1f;
        }
    }
}
