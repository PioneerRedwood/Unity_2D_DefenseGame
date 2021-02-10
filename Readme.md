# 2D Tower Defense Game by Unity

2020.12.22 ~ 진행 중 (with Lulie)

스타크래프트 1과 2의 랜덤타워디펜스 유즈맵을 모토로 삼아 개발했다.

------

초반 3주동안은 기획과 개발을 동시에 하려고 다른 게임 조사를 하느라 시간을 많이 썼다.

### #1. 2020.12.22 ~ 2020.12.27

------

### #2. 2020.12.28 ~ 2021.01.03

------

### #3. 2021.01.04 ~ 2021.01.10

#### 2021.01.06 

​	Tile, Monster, Stage, StageManager, TileManager, StageEditor, TileEditor 추가

- Tile
  - 타일 기반으로 타워를 설치
  - Ground & Route
- StageEditor & TileEditor
  - Unity Editor내에서 스테이지와 타일을 편리하게 추가하기 위한 메타 클래스
  -  반드시 Assets/Editor에 위치해야함
  
  

#### 2021.01.07 

​	GamePrefabManager, SingletonScriptableObject, SimpleMonster, Player 추가

- GamePrefabManager 
  - 어떠한 클래서에서도 원하는 오브젝트를 생성하기 위해 몇몇 프리팹을 지정해둔 매니저 클래스
  - 반드시 Assets/Resources 디렉토리에 위치해야함
- SingletonScriptableObject
  -  GamePrefabManager가 게임 내 항상 하나의 인스턴스로만 존재하기 위한 추상 래퍼 클래스
  - UnityEngine.ScriptableObject를 상속



#### 2021.01.09

​	~~EndPoint, StartPoint,~~ Ground, Route, Tower, Wave, ObjectDetector, UIManager 추가

- Ground, Route

  - 평지와 경로, 평지에는 타워 건설, 경로는 몬스터가 다니는 길이며 장애물 건설 가능

- Tower

  - 모든 타워의 부모 클래스
  - 티어가 존재하며 던전앤파이터의 아이템 등급을 참고

- Wave

  - 웨이브를 두어 해당 스테이지에서 쓸 몬스터와 수를 담은 클래스
  - 만약 내부 값들을 변경하지 않는다면 구조체로 정의하여 클래스보다 효율적으로 사용 가능

- ObjectDetector

  - Tag에 맞게 오브젝트를 탐지

- UIManager

  - 선택한 오브젝트의 태그에 따라 상호작용

  

#### 2021.01.10

​	.gitignore 추가

------

### #3. 2021.01.11 ~ 2021.01.17

#### 2021.01.11

​	~~TileManager~~

​	본 프로젝트에서 사용하지 않음



#### 2021.01.14

​	StageContentViewer, StartSceneGameManager, Player, Bullet, 

- StageContentViewer (UI)
  - 스테이지 선택시 스테이지 전시와 선택을 담당한 클래스
  - 시간을 꽤 많이 들였는데 결과가 영 맘에 들지 않았다

- Player
  - 게임 내 하나의 인스턴스로만 존재하는 플레이어 클래스
  - 게임 진행 시 필요한 정보를 담고 있어 어떤 클래스에서도 접근이 가능하다
- Bullet
  - 기본적인 Bullet tower가 공격할때 생성되는 총알(Projectile)
  - 생성 시에 적에 대한 위치가 저장되고 그 위치까지 일정한 속도로 날아가는 방식으로 구현(후에 문제가 발생해 수정이 많이 진행됨)



#### 2021.01.16

​	TowerManager, StageViewer, ~~StageSelectCanvas~~, StageContentViewer -> StageSelector

​	StageDataHolder

- TowerManager
  
  - 스테이지마다 티어 별 사용가능한 타워를 담아놓은 클래스
- StageContentViewer -> StageSelector
  
  - 스테이지 선택하는 클래스에 걸맞도록 이름 변경
- StageViewer
  
  - 스테이지 정보를 보여주는 래퍼 클래스
- StageDataHolder
  - 스테이지 선택 씬에서 게임 씬으로 변경될 때 선택한 스테이지에 대한 정보를 스테이지 클래스에 전달하기 위한 중간 클래스
  - DontDestroyOnLoad로 선언됨
  
  

------

### #4. 2021.01.18 ~ 2021.01.24

#### 2021.01.19

​	InfoPanel

- ​	선택한 오브젝트에 대한 정보를 UIManager로부터 받아와 전시



#### 2021.01.20

​	FasterMoster, BulletTower, TowerAttackRange

- FasterMonster
  - 일반적으로 이동속도가 빠른 몬스터, 공격 받으면 이동속도가 감소
- BulletTower
  - 기본적인 총알 발사 타워
- TowerAttackRange
  - 타워 공격 범위 보여주는 클래스



#### 2021.01.21

​	AOETower, LaserTower

- AOETower
  - AOE(Area Of Effect). 범위 공격인 총알을 발사하는 타워
- LaserTower
  - 레이저를 쏘는 타워



#### 2021.01.22

- AOEBullet
  - AOETower에서 쏘는 총알



#### 2021.01.23

- BossMonster
  - 일반적으로 체력이 높지만 이동속도가 낮음



------

### #5. 2021.01.25 ~ 2021.01.31

#### 2021.01.25

​	BuffTower, SlowTower

- BuffTower
  - 일정 범위 내 타워의 공격력 증가
- SlowTower
  - 일정 범위 내 몬스터의 이동속도 감소



#### 2021.01.30

​	2D_DefenseGame.sln 프로젝트 파일 추가



------

### #6. 2021.02.01 ~ 2021.02.07

#### 2021.02.04.

​	Obstacle, MissionHolder, MissionManager

- Obstacle
  - 몬스터가 다니는 경로에 설치할 수 있는 장애물
  - 체력을 가지며 몬스터가 붙으면 점차 체력이 감소
- MissionHolder, MissionManager
  - 타워의 개수를 파악해 특정 미션을 달성하면 돈을 획득

------

:beers: Lulie는 더이상 프로젝트에 참여하지 않기로 함

### #7. 2021.02.08 ~ 2021.02.14

#### 2021.02.08

​	Android & WebGL 빌드

- PC에서 실행하는 것과 다르게 제한된 자원 내에서 실행하면 다소 프레임이 저하되는 현상이 존재
- 해상도가 맞지 않아 안드로이드는 UI가 작게 보이고 WebGL에선 해상도를 고정해야만 원활하게 동작



#### 2021.02.09

- 일부 클래스 멤버 변수 접근자 수정
- 타워 공격 범위 UI 수정
- 이전 스테이지를 클리어하지 않았을 경우 다음 스테이지 도전 못하도록 수정
- 장애물 설치 비용 ($20) 추가



추가 예정 항목

1. 스테이지 클리어 시 다음 스테이지로 넘어갈지 스테이지 선택 창으로 갈지 결정하는 패널 열리는 기능
2. 타워 업그레이드 효율 밸런싱



#### 2021.02.10

- 일부 클래스 멤버 변수 접근자 수정
- Player::up:Tower Upgrade 방식 수정 및 업그레이드 효율 밸런싱
- 스테이지 클리어 시 Next, Retry, Main 3개의 버튼으로 다음 행동 제어
- 미션 작동 방식 대폭 수정
  - 효율적인 설계에 관해 논의 필요



추가 예정 항목

1. 클래스 디렉토리 리팩토링 -> 처음 설계부터 잘 해야 이런 일이 발생하지 않음 🤐



치명적인 버그 발생 - 