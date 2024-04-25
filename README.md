## 1. 프로젝트 설명
OverCooked2를 모작하여 만든 프로젝트입니다.   
제한 시간내에 주문서에 나오는 음식을 최대한 많이 반납하여 점수를 올리는 액션-캐쥬얼 게임입니다.   

## 2. 개발 환경
- IDE: Visual Studio 2019   
- Unity 버전: 2021.3.24f1   
- VCS: Git, Github   
  - GUI 툴: SourceTree   

## 3. 코드 설명
/UnderCooked/Assets/Scripts 폴더에 코드들이 저장돼있습니다.   
### 3.1 Animation
카메라 움직임, 화면 전환, 물체의 반복적인 움직임 등의 애니메이션을 코드로 만들어 모아둔 폴더입니다.   
### 3.2 FSM
Player의 상태를 FSM(Finite State Machine)을 사용해 만든 코드를 모아둔 폴더입니다.
Player, BaseState, StateMahcine으로 구성되며, /FSM/States 폴더에는 프로젝트에 사용되는 Player의 상태 코드가 있습니다.
### 3.3 Managers
싱글톤 패턴을 사용해 자주 사용되는 요소들을 Manager로 저장한 폴더입니다.
### 3.4 Objects
Player와 상호작용되는 Item들에 대한 코드를 모아둔 폴더입니다. (ex. 접시 반납대, 접시 스포너, 쓰레기통 등)   
### 3.5 UI
처음 씬부터 마지막 씬까지의 UI를 컨트롤하는 코드를 모아둔 폴더입니다.   

## 4. 프로젝트 진행 방법
### 4.1 조작법
- 이동: 방향키(←↑↓→)   
- 아이템 상호작용: Space   
- 스킬(썰기): Left Ctrl   
- 대쉬: Left Alt   

### 4.2 진행과정
exe 파일 실행 --> 게임 스테이지 선택 --> 게임 진행 --> 다시하기 or 메인화면으로 돌아가기   
