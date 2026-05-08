# AI Hegemony Wars (AIWars-Unity)

LLM 벤치마크 데이터로 싸우는 3D RTS/FPS 하이브리드.
한반도 튜토리얼 → 글로벌 지구본 전장. 미국(GPT/Claude/Gemini) vs 중국(DeepSeek/Qwen/Kimi) + AI vs AI 난투.

> 기획서 기반 / `.md` 에이전트 분담 작성 / 한국어·영어 토글 지원.

## 1. 사전 준비물

| 항목 | 버전 / 비고 |
|---|---|
| **Unity Hub** | 최신 |
| **Unity Editor** | **2022.3.40f1 LTS** (`ProjectSettings/ProjectVersion.txt` 고정) |
| **모듈** | Windows Build Support (IL2CPP), URP 템플릿 권장 |
| **선택** | Unity Account + UGS 프로젝트 ID (멀티플레이어용) |

## 2. 5분 실행 가이드 (싱글플레이 / 한국어)

1. Unity Hub 실행 → **Open** → 이 폴더(`AIWars-Unity`) 선택.
2. 처음 열면 패키지가 자동 다운로드됨 (`Packages/manifest.json`에 명시된 NGO, Cinemachine, Input System, UGS 등). 5–10분 소요.
3. 콘솔에 `Recommended fix` 다이얼로그가 뜨면 **Apply** 클릭 (Input System).
4. 메뉴: **AIWars → Seed LLM Unit Data From CSV** 클릭 → 14개 LLM 유닛 ScriptableObject 자동 생성.
5. 메뉴: **AIWars → Validate LLM Unit Data** 로 데이터 검증.
6. `Assets/Scenes/Bootstrap.unity` 더블클릭 → ▶ **Play** 누르면 한국어 UI로 시작.

> Bootstrap 씬은 직접 만들어야 함 (아래 3절).

## 3. Bootstrap 씬 만들기 (1회 작업, 약 10분)

빈 씬에서 다음 GameObject만 생성하면 모든 시스템이 자동 연결됩니다.

```
Bootstrap (빈 씬)
├── _GameManager          (스크립트: GameManager)
├── _Localization         (스크립트: LocalizationManager)
├── _Audio                (스크립트: AudioManager)
├── _Buffs                (스크립트: RegionalBuffSystem - usaFaction/chinaFaction 할당)
├── _Drift                (스크립트: ModelVersionDriftSystem)
├── RTSCameraRig          (Camera + RTSCameraController, height=80, rot=(60,0,0))
├── FPSCameraRig          (Camera, 비활성화)
├── CameraManager         (스크립트: CameraManager — rts/fps 두 카메라 할당)
├── Player                (스크립트: PossessionController + RTSSelectionController)
├── HUDCanvas             (Canvas + LanguageToggle + HUDController + LocalizedText들)
└── Terrain               (Terrain — 한반도 heightmap 임포트)
```

각 시스템은 `[CreateAssetMenu]` ScriptableObject로 데이터를 받습니다:
- **Assets/Right Click → Create → AIWars → Faction Data** → `Faction_USA.asset`, `Faction_China.asset` 생성.
- **Create → AIWars → Region Data** → `Region_Korea.asset` 등 (tag에 `AmericasContinent`/`AsiaContinent`).
- **Create → AIWars → Variables → Resource Pool** → `Resources_USA.asset`, `Resources_China.asset`.

## 4. 영어 플레이 (English Play Mode)

게임 내에서 **언어 토글 버튼**을 누르면 즉시 English UI로 전환.

```
LanguageToggle.cs   ──  Korean / English 버튼
LocalizationManager ──  Resources/Localization/ko.json | en.json 로드
LocalizedText       ──  Text/TMP_Text 컴포넌트 자동 갱신
```

- **저장**: 선택한 언어는 `PlayerPrefs`로 저장되어 다음 실행 시 유지.
- **확장**: `Assets/Resources/Localization/` 에 `ja.json`, `zh.json` 추가하면 다른 언어도 즉시 지원.
- **에이전트 멘토 음성**: 영어 모드에서는 모든 NPC 음성도 영어 라인으로 (오디오 파일은 본인이 추가).

기획서의 *"영어로 플레이"* 요구는 두 가지 의미로 구현했습니다:
1. **UI 언어 토글** (위 — 즉시 사용 가능)
2. **영어 학습 모드 훅** — `LocalizationKeys` 상수로 키 일괄 관리, 키마다 영문 단어 옆에 한글 정의를 툴팁으로 함께 보여주는 `LocalizedTextWithGloss` 확장은 `docs/15-english-mode.md` 참고.

## 5. 멀티플레이어 활성화

기본 프로젝트는 NGO/UGS 코드를 `#if NGO` / `#if UGS` 가드로 비활성화 상태로 컴파일합니다 (싱글플레이 빌드 가능).

활성화 단계:
1. **Window → Package Manager** 에서 다음 패키지 설치 확인:
   - Netcode for GameObjects
   - Authentication / Lobby / Relay (UGS)
2. **Project Settings → Player → Scripting Define Symbols** 에 추가:
   ```
   NGO;UGS
   ```
3. **Edit → Project Settings → Services** 에서 UGS 프로젝트 ID 연결.
4. `LobbyController.InitializeAsync()` 가 **Start** 시 호출되도록 메뉴 씬에 컴포넌트 추가.

## 6. 조작법

| 시점 | 키 | 동작 |
|---|---|---|
| RTS | 좌클릭 | 유닛 선택 |
| RTS | 우클릭 | 이동 명령 |
| RTS | 화살표 / 화면 가장자리 | 카메라 팬 |
| RTS | 마우스 휠 | 카메라 줌 |
| RTS | **C** | 마우스 위 유닛 빙의 |
| FPS | **W A S D** | 이동 (50% 버프 적용) |
| FPS | 마우스 | 시야/조준 |
| FPS | 좌클릭 | 발사 (50% 데미지 버프) |
| FPS | 우클릭 | ADS (확장 예정) |
| FPS | **V** | 빙의 해제 → RTS 복귀 |

## 7. 폴더 구조

```
Assets/
├── Scripts/
│   ├── Core/             GameManager, ServiceLocator
│   ├── Data/             ScriptableObject 정의 (LLMUnitData 등)
│   ├── Units/            Unit, Health, Locomotion, Combat, ActionValue, PossessionTarget
│   ├── Camera/           CameraManager (RTS↔FPS 전환)
│   ├── Player/           PossessionController, RTSSelection, RTSCamera
│   ├── Combat/           UnitProductionFacility
│   ├── World/            RegionController, RegionalBuffSystem
│   ├── Time/             ModelVersionDriftSystem (Claude 환각 / GPT 스로틀링)
│   ├── Multiplayer/      NetcodeBootstrap, LobbyController, DeterministicLockstep
│   ├── Localization/     LocalizationManager, LocalizedText, JsonHelper
│   ├── Audio/            AudioManager
│   ├── UI/               HUDController, LanguageToggle
│   └── Editor/           LLMUnitDataValidator, SeedLLMUnitsFromCsv
├── Resources/
│   └── Localization/     ko.json, en.json
├── GameData/
│   ├── llm_units_seed.csv
│   └── LLMUnits/         (자동 생성된 .asset 파일들)
└── Scenes/
    └── Bootstrap.unity   (직접 생성)
```

## 8. 시스템별 담당 에이전트 (.md 매핑)

| 시스템 | 담당 에이전트 | 핵심 파일 |
|---|---|---|
| 데이터 ScriptableObject | Unity Architect 🏛️ | `Data/*.cs` |
| 유닛/전투/이동 | Unity Architect + Game Designer | `Units/*.cs` |
| 카메라 RTS↔FPS | Unity Architect | `Camera/CameraManager.cs` |
| 빙의 + 50% 버프 | Game Designer 🎮 | `Player/PossessionController.cs` |
| 지역 버프 | Game Designer | `World/RegionalBuffSystem.cs` |
| 버전 드리프트 | Game Designer + AI Engineer 🤖 | `Time/ModelVersionDriftSystem.cs` |
| 멀티플레이어 | Unity Multiplayer Engineer 🔗 | `Multiplayer/*.cs` |
| 한↔영 전환 | UI Designer 🎨 + CMS Developer 🧱 | `Localization/*.cs`, `Resources/Localization/*.json` |
| 사운드 | Game Audio Engineer 🎵 | `Audio/AudioManager.cs` |
| 에디터 자동화 | Unity Editor Tool Developer 🛠️ | `Editor/*.cs` |
| 데이터 시드 | Data Engineer 🔧 | `GameData/llm_units_seed.csv` |
| 백엔드/네트워크 | Backend Architect 🏗️ | `docs/06-multiplayer-architecture.md` |
| 게임 디자인 / 내러티브 / 레벨 | 각 분야 디자이너 | `docs/01–03` |
| 비주얼 프롬프트 | Image Prompt Engineer 📷 | `docs/14-image-prompts.md` |
| UX 폰더 + 디자인 토큰 | UX Architect 📐 + UI Designer | URP 머티리얼은 직접 추가 |
| 셰이더/VFX | Unity Shader Graph Artist ✨ | `docs/07-shader-pipeline.md` (TBD) |
| 테크 아트 예산 | Technical Artist 🎨 | `docs/04-art-pipeline.md` (TBD) |

## 9. 트러블슈팅

| 증상 | 해결 |
|---|---|
| `Cinemachine` 네임스페이스 못 찾음 | Player Settings → Scripting Define에 `CINEMACHINE` 추가 (자동) |
| 컴파일 에러: `Unity.Netcode` | Package Manager에서 NGO 설치 |
| 한국어/영어 토글이 안 됨 | `Resources/Localization/{ko,en}.json` 둘 다 존재 확인 |
| 빙의 시 카메라가 따라오지 않음 | `PossessionTarget.headBone` 할당 확인 |
| 빌드 시 `LLMUnitData` 검증 실패 | **AIWars → Validate LLM Unit Data** 로 어떤 에셋이 깨졌는지 확인 |

## 10. 다음 단계 (TODO)

- [ ] 한반도 heightmap (`SkyDark Heightmap` 등) 임포트
- [ ] URP 머티리얼 + 진영별 셰이더
- [ ] FMOD 또는 Wwise 통합 (현재는 AudioSource 풀백)
- [ ] NGO 락스텝 → 실제 ServerRpc 브로드캐스트 연결
- [ ] 튜토리얼 컷씬 + 서사 라인
- [ ] 글로벌 지구본 셰이더 (구체 메시 + 위경도→3D 변환)
