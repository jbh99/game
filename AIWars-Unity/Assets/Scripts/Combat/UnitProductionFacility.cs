using UnityEngine;
using AIWars.Data;
using AIWars.Units;
using AIWars.World;

namespace AIWars.Combat
{
    // Spawns LLM-unit prefabs from a faction roster, gated by Action Value cadence and resources.
    public class UnitProductionFacility : MonoBehaviour
    {
        [SerializeField] FactionData faction;
        [SerializeField] PlayerResourcePool wallet;
        [SerializeField] Transform spawnPoint;
        [SerializeField] RegionController homeRegion;
        [SerializeField] LLMUnitData currentOrder;
        float _progress;

        public void Order(LLMUnitData unit) { currentOrder = unit; _progress = 0f; }

        void Update()
        {
            if (currentOrder == null) return;

            float productionMul = RegionalBuffSystem.Instance != null
                ? RegionalBuffSystem.Instance.GetProductionMultiplier(faction.faction, homeRegion)
                : 1f;
            // Lower AV = faster build. Production speed scales 1/AV.
            float ratePerSec = (1000f / currentOrder.actionValue) * productionMul;
            _progress += ratePerSec * Time.deltaTime;

            if (_progress >= 100f && wallet != null)
            {
                int cost = Mathf.RoundToInt(currentOrder.unitCost
                    * (RegionalBuffSystem.Instance != null
                        ? RegionalBuffSystem.Instance.GetCostMultiplier(faction.faction, homeRegion)
                        : 1f));
                if (wallet.TrySpend(cost))
                {
                    SpawnUnit();
                    _progress = 0f;
                }
                else
                {
                    _progress = 99f; // wait for funds
                }
            }
        }

        void SpawnUnit()
        {
            if (currentOrder.prefab == null || spawnPoint == null) return;
            var go = Instantiate(currentOrder.prefab, spawnPoint.position, spawnPoint.rotation);
            if (go.TryGetComponent(out Unit unit)) unit.Configure(currentOrder);
        }
    }
}
