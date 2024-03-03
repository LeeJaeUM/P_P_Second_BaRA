# P_P_Second_BaRA
 Atents 3D Project
 
### 프로젝트 소개 
3인칭 3D 액션게임 구현
![프로젝트2처음-Thumbnail (2)](https://github.com/LeeJaeUM/P_P_Second_BaRA/assets/106094800/14e1c177-d29e-4fbf-b327-c9c7586b6506)


-----

### 설명 영상 링크 :  https://youtu.be/kFxZ9MI73E8
----

### 플레이 방법
WASD : 이동

마우스 좌클릭 : 공격
마우스 우클릭 : 가드/패리

ctrl 입력 : 이모션 모드로 변경/해제
ctrl 입력 후 숫자 키 : 이모션 (현재는 숫자 5만 가능)

shift : 대시

## 중요 소스코드 및 코드 설명
- Player
   - 인풋시스템 입력 받고 모든 동작 구현
- HitCollider
  - 피격 시 위치와 공격한 적의 데미지, 데미지 배율, 공격타입을 받아 PlayerHit로 넘김 
- PlayerHit
   - HitCollider에서 받아온 정보를 처리하여 밀쳐지는 거리 조절 및 피격 가능한지 판단
- EnemyBase
    - 적들이 가지는 기본 정보
- Enemy
    - 적의 AI구현. 이동 및 공격 패턴 구현 EnemyBase를 상속받음
- AttackAble
    - 공격 가능한 물체를 정의하는 클래스. 콜라이더를 받아 공격 범위를 정함
- Weapon
    - AttackAble를 상속받는 플레이어 무기 코드. 
- HPGuage, NanoGauge
    - 플레이어의 체력 게이지와 패링게이지를 화면에 표현.
