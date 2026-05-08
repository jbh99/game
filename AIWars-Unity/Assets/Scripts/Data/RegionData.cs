using UnityEngine;

namespace AIWars.Data
{
    [CreateAssetMenu(menuName = "AIWars/Region Data", fileName = "Region_")]
    public class RegionData : ScriptableObject
    {
        public string regionId;
        public string displayName;
        [TextArea] public string description;

        [Header("Resource yield per turn / minute")]
        public int resourceYield = 50;

        [Header("World position (planet sphere local coords)")]
        public Vector3 anchorPoint;
        public float boundingRadius = 50f;

        [Header("Special tags for buff lookup (e.g. AmericasContinent, AsiaContinent)")]
        public string[] tags;
    }
}
