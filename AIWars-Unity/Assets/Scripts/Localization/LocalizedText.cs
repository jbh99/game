using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AIWars.Localization
{
    [DisallowMultipleComponent]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] string key;
        Text _legacy;
        TMP_Text _tmp;

        void Awake()
        {
            _legacy = GetComponent<Text>();
            _tmp = GetComponent<TMP_Text>();
        }

        void OnEnable()
        {
            if (LocalizationManager.Instance != null)
            {
                LocalizationManager.Instance.OnLanguageChanged += Apply;
                Apply();
            }
        }

        void OnDisable()
        {
            if (LocalizationManager.Instance != null)
                LocalizationManager.Instance.OnLanguageChanged -= Apply;
        }

        void Apply()
        {
            string v = LocalizationManager.Instance.Get(key);
            if (_legacy != null) _legacy.text = v;
            if (_tmp != null)    _tmp.text    = v;
        }

        public void SetKey(string newKey)
        {
            key = newKey;
            Apply();
        }
    }
}
