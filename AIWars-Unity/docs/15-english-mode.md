# English Play Mode (영어 플레이 기능)

The plan asked for "영어로 플레이하는 영어 플레이 기능." We interpret this two ways and ship both.

## Mode A — UI/Voice Localization Toggle (default)
Click the **English** button in the language toggle. The entire UI swaps to English instantly via:
- `LocalizationManager` reads `Resources/Localization/en.json`
- All `LocalizedText` components subscribe to `OnLanguageChanged` and re-render
- `PlayerPrefs("aiwars.lang")` persists choice across sessions
- Audio voice-over: hook a per-language `AudioMixerSnapshot` so future VO recordings can swap tracks

## Mode B — English Learning Overlay (optional)
For Korean players who want to play *in English to study English*, enable a glossing layer.

### Add `LocalizedTextWithGloss.cs` (drop-in)

```csharp
using UnityEngine;
using TMPro;
using AIWars.Localization;

namespace AIWars.Localization
{
    public class LocalizedTextWithGloss : MonoBehaviour
    {
        [SerializeField] string englishKey;     // "ui.start_singleplayer"
        [SerializeField] string koreanGlossKey; // "ui.start_singleplayer.gloss"
        [SerializeField] TMP_Text label;

        void OnEnable()  => LocalizationManager.Instance.OnLanguageChanged += Refresh;
        void OnDisable() => LocalizationManager.Instance.OnLanguageChanged -= Refresh;

        void Start() => Refresh();

        void Refresh()
        {
            string en = LocalizationManager.Instance.Get(englishKey);
            if (LocalizationManager.Instance.Current == Language.Korean)
            {
                string ko = LocalizationManager.Instance.Get(koreanGlossKey);
                label.text = $"{en}  <size=60%><color=#888>({ko})</color></size>";
            }
            else
            {
                label.text = en;
            }
        }
    }
}
```

### Add gloss keys to `ko.json`
```json
"ui.start_singleplayer.gloss": "혼자서 플레이",
"hud.possess_buff.gloss":      "빙의 시 강화 효과"
```

### How players use it
- **Pure English mode:** click English toggle → no gloss, immersive English.
- **Bilingual study mode:** stay in Korean toggle but use `LocalizedTextWithGloss` on key UI → both languages shown side-by-side.

## Voice line guidance (for future audio recording)
When recording English VO, follow the Narrative Bible character voice pillars:
- Reyes: clipped, no contractions ("Run the numbers")
- Mei: long sentences, no first-person pronouns
- Mentor: ironic, verbose, occasional self-contradiction

## Future
- Add `ja.json`, `zh-CN.json`, `zh-TW.json` — system already supports them.
- Add a `LanguageDetector` that defaults to system locale on first launch.
- Wire FMOD events to localize SFX (currency announcements, region names spoken in unit's faction language).
