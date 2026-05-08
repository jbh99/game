using System.Collections.Generic;
using UnityEngine;

namespace AIWars.Audio
{
    // Lightweight audio façade so the rest of the code doesn't talk to AudioSource directly.
    // Real shipping build: replace with FMOD/Wwise event references per Audio Engineer agent.
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [System.Serializable]
        public struct ClipEntry { public string key; public AudioClip clip; }

        [SerializeField] ClipEntry[] clips;
        Dictionary<string, AudioClip> _table;
        AudioSource _2dSource;

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _table = new Dictionary<string, AudioClip>();
            foreach (var c in clips) if (c.clip != null) _table[c.key] = c.clip;

            _2dSource = gameObject.AddComponent<AudioSource>();
            _2dSource.spatialBlend = 0f;
        }

        public void PlayOneShot(string key, Vector3 worldPos)
        {
            if (!_table.TryGetValue(key, out var clip) || clip == null) return;
            AudioSource.PlayClipAtPoint(clip, worldPos);
        }

        public void PlayUI(string key)
        {
            if (!_table.TryGetValue(key, out var clip) || clip == null) return;
            _2dSource.PlayOneShot(clip);
        }
    }
}
