using UnityEngine;
using AIWars.Data;
using AIWars.Units;

namespace AIWars.World
{
    // Applies the plan's regional home buffs:
    //   USA in Americas: +20% production, +40% damage
    //   China in Asia:   +30% production, -40% cost (cheaper)
    public class RegionalBuffSystem : MonoBehaviour
    {
        public static RegionalBuffSystem Instance { get; private set; }
        [SerializeField] FactionData usaFaction;
        [SerializeField] FactionData chinaFaction;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

        public float GetDamageMultiplier(Unit unit) => GetMul(unit, m => m.damageMul, 1f);
        public float GetProductionMultiplier(Faction f, RegionController region) => GetProduction(f, region);
        public float GetCostMultiplier(Faction f, RegionController region)
        {
            FactionData fd = Resolve(f);
            if (fd == null || region == null) return 1f;
            if (!region.ContainsTag(fd.homeRegionId)) return 1f;
            return fd.costMul;
        }

        float GetProduction(Faction f, RegionController region)
        {
            FactionData fd = Resolve(f);
            if (fd == null || region == null) return 1f;
            if (!region.ContainsTag(fd.homeRegionId)) return 1f;
            return fd.productionSpeedMul;
        }

        float GetMul(Unit unit, System.Func<FactionData, float> sel, float fallback)
        {
            if (unit == null || unit.Data == null) return fallback;
            FactionData fd = Resolve(unit.Faction);
            if (fd == null) return fallback;
            // Only buff if unit is currently inside a home-region collider.
            foreach (var r in RegionController.All)
                if (r.ContainsTag(fd.homeRegionId) && Vector3.Distance(unit.transform.position, r.transform.position) < r.Region.boundingRadius)
                    return sel(fd);
            return fallback;
        }

        FactionData Resolve(Faction f) => f switch
        {
            Faction.USA => usaFaction,
            Faction.China => chinaFaction,
            _ => null
        };
    }
}
