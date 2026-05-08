using System.Collections.Generic;
using UnityEngine;
using AIWars.Data;

namespace AIWars.World
{
    public class RegionController : MonoBehaviour
    {
        [SerializeField] RegionData region;
        [SerializeField] Faction owner = Faction.Neutral;
        [SerializeField] PlayerResourcePool[] pools;
        [SerializeField] float yieldIntervalSeconds = 30f;

        public RegionData Region => region;
        public Faction Owner => owner;
        public bool ContainsTag(string tag)
        {
            if (region == null || region.tags == null) return false;
            foreach (var t in region.tags) if (t == tag) return true;
            return false;
        }

        float _timer;

        void Update()
        {
            if (owner == Faction.Neutral || region == null) return;
            _timer += Time.deltaTime;
            if (_timer < yieldIntervalSeconds) return;
            _timer = 0f;
            foreach (var p in pools) if (p != null) p.Add(region.resourceYield);
        }

        public void SetOwner(Faction f) => owner = f;

        // Returns all regions present in the scene (cheap for ≤ a few hundred).
        public static List<RegionController> All { get; } = new();
        void OnEnable() => All.Add(this);
        void OnDisable() => All.Remove(this);
    }
}
