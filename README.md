# FlappyStar (수정중)

## 1. 개요
Unity로 제작한 플래피 버드 스타일의 미니 프로젝트입니다.<br>
프로젝트 자체 로직보다는 코어 패키지로 최대한 빠르게 완성하는 것을 목표로 잡았습니다.

## 2. 시연 영상
(추가 예정)

## 3. 다운로드
[빌드 다운로드(Itch.io 링크)](https://fortis001.itch.io/flappystar)<br>
자세한 플레이 설명은 Itch 페이지에 있습니다.

## 4. 개발 환경
- Unity 6000.0.44f1
- C# / Visual Studio Code

## 5. 주요 기능
### 5.1 장애물 조립 및 생성
장애물 부품이 다른 스프라이트와 내부 개체를 가진 프리팹으로 구성되어 있어, 다중 프리팹 풀을 이용해 풀링 후 조립한다.
- 플레이어의 크기와 여윳값을 기준으로 통과 가능 영역 계산
- 진행 시간에 따라 여윳값 스케일링으로 난이도 조정
- 중간의 통과 가능한 영역 생성
- 장애물 부품 프리팹 배리언트를 뽑아 조립
- 오브젝트 풀링으로 지나간 장애물 분해 후 반환
### 5.2 커스텀 타임 매니저와 게임 오버 연출


## 6. 아키텍처


## 7. 샘플 코드
[ObstacleManager](Assets/2_Scripts/2_Gameplay/Scene/InGame/ObstacleManager.cs)<br>
[ObstaclePair](Assets/2_Scripts/2_Gameplay/Entities/ObstaclePair.cs)

## 8. 고민했던 부분들


## 9. 개선 가능한 부분


