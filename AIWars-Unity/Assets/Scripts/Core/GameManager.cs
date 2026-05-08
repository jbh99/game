using UnityEngine;
using AIWars.Localization;
using AIWars.World;
using AIWars.TimeSystems;

namespace AIWars.Core
{
    // Single bootstrap that wires up the cross-cutting singletons.
    [DefaultExecutionOrder(-100)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        [SerializeField] GameObject localizationPrefab;
        [SerializeField] GameObject regionalBuffPrefab;
        [SerializeField] GameObject driftSystemPrefab;
        [SerializeField] GameObject audioManagerPrefab;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            EnsureSingleton<LocalizationManager>(localizationPrefab);
            EnsureSingleton<RegionalBuffSystem>(regionalBuffPrefab);
            EnsureSingleton<ModelVersionDriftSystem>(driftSystemPrefab);
            EnsureSingleton<AIWars.Audio.AudioManager>(audioManagerPrefab);
        }

        static void EnsureSingleton<T>(GameObject prefab) where T : Component
        {
            if (Object.FindFirstObjectByType<T>() != null) return;
            if (prefab != null) Instantiate(prefab);
            else
            {
                var go = new GameObject(typeof(T).Name);
                go.AddComponent<T>();
                DontDestroyOnLoad(go);
            }
        }
    }
}
