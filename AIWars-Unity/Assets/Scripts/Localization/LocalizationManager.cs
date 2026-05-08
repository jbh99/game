using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIWars.Localization
{
    public enum Language { Korean, English }

    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance { get; private set; }
        public event Action OnLanguageChanged;

        public Language Current { get; private set; } = Language.Korean;

        Dictionary<string, string> _table = new();

        void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            string saved = PlayerPrefs.GetString("aiwars.lang", "ko");
            SetLanguage(saved == "en" ? Language.English : Language.Korean);
        }

        public void SetLanguage(Language lang)
        {
            Current = lang;
            string code = lang == Language.English ? "en" : "ko";
            PlayerPrefs.SetString("aiwars.lang", code);

            var asset = Resources.Load<TextAsset>($"Localization/{code}");
            if (asset == null)
            {
                Debug.LogError($"[Loc] Missing Resources/Localization/{code}.json");
                return;
            }
            _table = JsonHelper.ParseFlatJson(asset.text);
            OnLanguageChanged?.Invoke();
        }

        public string Get(string key)
        {
            if (_table.TryGetValue(key, out string v)) return v;
            return $"[{key}]";
        }

        public string Format(string key, params object[] args)
        {
            string raw = Get(key);
            try { return string.Format(raw, args); }
            catch { return raw; }
        }
    }
}
