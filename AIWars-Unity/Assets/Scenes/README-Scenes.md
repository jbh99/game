# Scenes

Unity 씬 파일(.unity)은 바이너리 직렬화 + 자동 GUID라서 외부에서 직접 만들 수 없음.
**Unity Editor에서 다음 순서로 직접 만들어 주세요:**

1. **File → New Scene** → Empty (Built-in)
2. 위 [README.md §3](../../README.md) 의 GameObject 트리대로 빈 GameObject들 추가
3. 각 GameObject에 명시된 스크립트 컴포넌트 부착
4. ScriptableObject 에셋들(`AIWars/Faction Data` 등)을 Inspector에서 슬롯에 드래그
5. **File → Save As** → `Assets/Scenes/Bootstrap.unity`
6. **File → Build Settings** → Bootstrap을 첫 씬으로 등록

이 단계는 한 번만 하면 됩니다. 이후 ▶ Play 시 모든 시스템이 자동으로 바인딩됩니다.
