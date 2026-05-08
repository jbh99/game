using UnityEngine;
using TMPro;
using AIWars.Data;
using AIWars.Localization;
using AIWars.Player;

namespace AIWars.UI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] PlayerResourcePool wallet;
        [SerializeField] TMP_Text resourceLabel;
        [SerializeField] TMP_Text possessHint;
        [SerializeField] PossessionController possession;

        void OnEnable()
        {
            if (wallet != null) { wallet.OnChanged += OnResourceChanged; OnResourceChanged(wallet.Credits); }
            if (LocalizationManager.Instance != null)
                LocalizationManager.Instance.OnLanguageChanged += Refresh;
            Refresh();
        }

        void OnDisable()
        {
            if (wallet != null) wallet.OnChanged -= OnResourceChanged;
            if (LocalizationManager.Instance != null)
                LocalizationManager.Instance.OnLanguageChanged -= Refresh;
        }

        void OnResourceChanged(int v)
        {
            if (resourceLabel != null && LocalizationManager.Instance != null)
                resourceLabel.text = LocalizationManager.Instance.Format("hud.resources", v);
        }

        void Refresh()
        {
            if (LocalizationManager.Instance == null) return;
            if (wallet != null) OnResourceChanged(wallet.Credits);

            if (possessHint != null)
            {
                bool isPossessing = possession != null && possession.PossessedUnit != null;
                possessHint.text = LocalizationManager.Instance.Get(
                    isPossessing ? "hud.possess_buff" : "hud.possess_hint");
            }
        }

        void Update() => Refresh();
    }
}
