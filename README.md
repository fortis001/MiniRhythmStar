# MiniRhythmStar

## 1. 개요
Unity로 제작한 리듬게임 포트폴리오 프로젝트입니다.
로컬 음원을 업로드해 비트맵을 직접 제작하고 플레이할 수 있습니다.

## 2. 시연 영상
(링크 추가 예정)

## 3. 다운로드
[빌드 다운로드(Itch.io 링크)](https://fortis001.itch.io/minirhythmstar)
자세한 플레이 설명은 Itch 페이지에 있습니다.

## 4. 개발 환경
- Unity 6000.0.44f1
- C# / Visual Studio Code
- Newtonsoft.Json
- StandaloneFileBrowser

## 5. 주요 기능
- 로컬 음원 기반 비트맵 에디터 (노트 배치, 저장, 로드)
- 난이도별 레인 구성 (Easy 3레인 / Normal 6레인 / Hard 9레인)
- 노트 판정 시스템 (Perfect / Great / Good / Bad / Miss)
- 콤보 및 스코어 시스템
- 결과 화면 및 플레이 기록 저장
- 키보드 단축키 지원 (ESC, Space 등으로 버튼 조작 가능)

## 6. 아키텍처
### 씬 초기화 구조
각 씬의 SceneManager가 전역 데이터 창구 역할을 담당합니다.
전역 객체(GameManager 등)에 대한 접근은 SceneManager에서만 이루어지고,
나머지 객체들은 SceneManager로부터 필요한 데이터를 전달받습니다.

### 씬 전환 시스템
TransitionManager와 SceneLoader의 역할을 분리했습니다.
- TransitionManager: 페이드 인/아웃 및 씬 전환 흐름 제어
- SceneLoader: 비동기 씬 로딩 및 진행률 표시
- 로딩 씬 사용 여부를 선택할 수 있는 구조
- Time.unscaledDeltaTime으로 일시정지 중에도 페이드 동작 보장

### 커스텀 ScrollView
Unity 기본 ScrollRect를 확장해 직접 구현했습니다.
- Idle / Dragging / Coasting / Snapping 상태 머신 기반
- AnimationCurve로 중앙 정렬 시 시각적 오프셋 제어
- 자동 스냅 기능

### 노트 시스템
- Queue 기반 노트 스케줄링으로 효율적인 타이밍 제어
- 오브젝트 풀링으로 노트 생성/파괴 비용 절감
- Newtonsoft.Json으로 차트 데이터 역직렬화

(다이어그램 추가 예정)

## 7. 샘플 코드
- [TransitionManager](Assets/2_Scripts/Core/Managers/TransitionManager.cs)
- [커스텀 ScrollView](Assets/2_Scripts/UI/Components/ScrollController.cs)

## 8. 고민했던 부분들
첫 개인 프로젝트인 만큼 코드 구조에 집중했습니다.

- **씬 초기화 구조**: 씬매니저가 전역 데이터 창구 역할을 담당하고, 각 객체는 GameManager를 직접 참조하지 않도록 설계했습니다.
- **이벤트 기반 구조**: 객체 간 직접 참조 대신 이벤트로 결합도를 낮췄습니다.
- **오브젝트 풀링**: 노트 생성/파괴 비용을 줄이기 위해 NotePoolManager를 구현했습니다.
- **ScriptableObject**: 난이도별 데이터를 코드에서 분리해 Inspector에서 관리했습니다.

## 9. 개선 가능한 부분
- 멀티 해상도 완전 지원
- 비트맵 에디터 편의 기능 (노트 다중 선택, 복사/붙여넣기 등)
- SoundManager의 클립 로딩 책임 분리
