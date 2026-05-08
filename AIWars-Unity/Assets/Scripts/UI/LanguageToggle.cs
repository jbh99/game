using UnityEngine;
using UnityEngine.UI;
using AIWars.Localization;

namespace AIWars.UI
{
    // Three-button language toggle: Korean, English, System (defaults to Korean).
    // Wire the buttons in the Inspector.
    public class LanguageToggle : MonoBehaviour
    {
        [SerializeField] Button koreanBtn;
        [SerializeField] Button englishBtn;

        void Start()
        {
            if (koreanBtn != null)  koreanBtn.onClick.AddListener(()  => SetLang(Language.Korean));
            if (englishBtn != null) englishBtn.onClick.AddListener(() => SetLang(Language.English));
        }

        void SetLang(Language lang)
        {
            if (LocalizationManager.Instance != null)
                LocalizationManager.Instance.SetLanguage(lang);
        }
    }
}
