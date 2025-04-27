## Part 1. 프로젝트 설명 / 레퍼런스 게임
OverCooked2를 모작하여 만든 프로젝트입니다.   
제한 시간내에 주문서에 나오는 음식을 최대한 많이 반납하여 점수를 올리는 액션-캐쥬얼 게임입니다.  
모든 기능은 구현하지는 않았으며 원작 스테이지 1-1을 기반으로 제작 하였다.

###  개발 환경
- IDE: Visual Studio 2019   
- Unity 버전: 2021.3.24f1   
- VCS: Git, Github   
  - GUI 툴: SourceTree   

###  조작법
- 이동: 방향키(←↑↓→)   
- 아이템 상호작용: Space   
- 스킬(썰기): Left Ctrl   
- 대쉬: Left Alt   

###  진행과정
exe 파일 실행 --> 게임 스테이지 선택 --> 게임 진행 --> 다시하기 or 메인화면으로 돌아가기  

## Part 2. 구현된 기능

![image](https://github.com/user-attachments/assets/31a4d37e-8b30-4a15-a47b-f234c9301afe)


#### Player AI
- Idle State  

  Key 입력이나 오브젝트를 들고 있을때 Move,Grab 상태로 변경
  <dr/>
- move State
  
  방향키의 키 입력이 들어오면 Speed값에 따른 움직임 이후 Idle 상태로 변경
  <dr/>
- Grab State

  오브젝트를 들고있다면 현재 상태를 Grab State로 변경시킨다
  <dr/>
- Chop State

  도마 위에 재료가 있다면 상호작용을 통해 Chop State로 변경, 재료 손질 완료 후 Idle 상태로 변경
  <dr/>

#### 가까운 오브젝트 찾기 기능  
- Player 기준으로 RayCast를 이용 하여 가까이 있는 Object의 머터리얼 색상 변경 및 탐지
- Object에 따라 재료생성, Drop, Table에 올리기 가능
  <dr/>

#### 쓰레기통  
- Player가 물체를 들고 있을 때 쓰레기통과 상호작용 시 Object 파괴
  <dr/>

#### 재료 손질 (도마)  
- 도마 위에 재료가 있다면 재료 손질 가능
- Player Animation Event를 사용 해 조건이 완료되면 손질된 재료 생성
- 손질 도중 벗어날 수 있으며 손질 완료되지 않은 재료는 Grab 불가능

  
