using UnityEngine;

namespace AIWars.Units
{
    // Implements the plan's Action Value formula: AV = 10000 / tokensPerSecond.
    // Lower AV = faster spawn cadence / faster attack tick.
    public class ActionValueSystem : MonoBehaviour
    {
        [SerializeField] float actionValue = 100f;
        [SerializeField] float gauge = 0f;
        public bool ActionReady => gauge >= actionValue;

        public void Initialize(float av) { actionValue = Mathf.Max(1f, av); gauge = 0f; }

        void Update()
        {
            // Tick: 1 unit of "intent" per second; faster models trigger sooner.
            gauge += 100f * Time.deltaTime;
        }

        public bool ConsumeAction()
        {
            if (!ActionReady) return false;
            gauge = 0f;
            return true;
        }

        public float Progress01 => Mathf.Clamp01(gauge / actionValue);
    }
}
