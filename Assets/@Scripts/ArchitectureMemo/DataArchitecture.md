{
  // --- 기본 정보 ---
  "id": "fire_ball",                      // 고유 식별자
  "dataId": 201001,                       // 데이터베이스 ID
  "name": "불공",                         // 공의 이름
  "description": "빠르고 화상 데미지를 주는 공",  // 설명
  "rarity": "rare",                       // 희귀도 (common, rare, epic, legendary)
  "element": "fire",                      // 원소 속성
  "level": 1,                             // 현재 레벨
  "maxLevel": 5,                          // 최대 레벨
  
  // --- 기본 스탯 ---
  "stat_baseSpeed": 6.5,                  // 기본 속도 - 발사 시 초기 속도
  "stat_baseDamage": 8,                   // 기본 데미지 - 블록 파괴 기본값
  "stat_baseMultiplier": 2,               // 기본 증폭기 - 수학 연산 기본 값
  "stat_maxHp": 900.0,                    // 최대 체력
  "stat_atk": 35.0,                       // 공격력
  "stat_atkBonus": 2.2,                   // 공격력 보너스 배율
  "stat_atkSpeed": 1.2,                   // 공격 속도 (초당 공격 횟수)
  "stat_atkTime": 0.9,                    // 공격 모션 시간 (초)
  "stat_atkRange": 4.0,                   // 공격 범위
  "stat_moveSpeed": 6.0,                  // 이동 속도
  "stat_power": 8,                        // 밀어내기 강도
  "stat_territory": 5,                    // 영역 범위
  "stat_criRate": 0.2,                    // 치명타 확률 (0~1)
  "stat_criDamage": 1.6,                  // 치명타 데미지 배율
  
  // --- Rigidbody 컴포넌트 속성 ---
  "rigidbody_mass": 0.7,                  // 질량 - 낮을수록 가벼움, 발사와 튕김에 영향
  "rigidbody_drag": 0.4,                  // 선형 항력 - 공기 저항, 이동 속도 감소 정도
  "rigidbody_angularDrag": 0.04,          // 회전 항력 - 회전 속도 감소 정도
  "rigidbody_useGravity": true,           // 중력 적용 여부 - true면 중력 영향 받음
  "rigidbody_isKinematic": false,         // 키네마틱 여부 - false면 물리 영향 받음
  "rigidbody_collisionDetectionMode": "Continuous", // 충돌 감지 모드 - 빠른 물체 정확한 충돌 계산
  "rigidbody_interpolation": "Interpolate", // 보간 방식 - 움직임 부드러움 설정
  "rigidbody_constraints": "None",        // 제약 조건 - 특정 축의 이동/회전 제한
  "rigidbody_maxAngularVelocity": 8.0,    // 최대 회전 속도 - 회전 속도 제한
  "rigidbody_maxDepenetrationVelocity": 12.0, // 최대 탈출 속도 - 물체 겹침 시 분리 속도
  
  // --- 물리 재질 속성 ---
  "physicsMaterial_bounciness": 0.8,      // 탄성 - 높을수록 잘 튕김 (0~1)
  "physicsMaterial_dynamicFriction": 0.5, // 동적 마찰 - 움직이는 물체의 마찰력
  "physicsMaterial_staticFriction": 0.5,  // 정적 마찰 - 정지 상태에서의 마찰력
  "physicsMaterial_frictionCombine": "Average", // 마찰 결합 방식
  "physicsMaterial_bounceCombine": "Maximum", // 탄성 결합 방식 - Maximum은 튕김 증가
  
  // --- 힘 및 속도 속성 ---
  "force_initialVelocityX": 0.0,          // 초기 X축 속도
  "force_initialVelocityY": 1.0,          // 초기 Y축 속도 - 위쪽 방향 초기 속도
  "force_initialVelocityZ": 0.0,          // 초기 Z축 속도
  "force_initialAngularVelocityX": 2.0,   // 초기 X축 회전 속도
  "force_initialAngularVelocityY": 2.0,   // 초기 Y축 회전 속도
  "force_initialAngularVelocityZ": 2.0,   // 초기 Z축 회전 속도
  "force_torqueMultiplier": 1.4,          // 회전력 계수 - 높을수록 회전 강해짐
  "force_forceMultiplier": 1.3,           // 힘 계수 - 모든 힘에 적용되는 배율
  "force_gravityScale": 0.95,             // 중력 스케일 - 1보다 작아 약간 가벼움
  "force_angleRange": 60,                 // 발사 각도 범위 - 플레이어 조준 가능 범위
  
  // --- 콜라이더 속성 ---
  "collider_type": "Sphere",              // 콜라이더 타입 - 구형 충돌체
  "collider_isTrigger": false,            // 트리거 여부 - false면 물리적 충돌 발생
  "collider_offsetX": 0.0,                // X축 오프셋
  "collider_offsetY": 0.5,                // Y축 오프셋 - 약간 위쪽으로 조정됨
  "collider_offsetZ": 0.0,                // Z축 오프셋
  "collider_radius": 0.5,                 // 반지름 - 충돌 감지 영역 크기
  "collider_physicsMaterial": "FireBallMaterial", // 적용할 물리 재질 이름
  
  // --- 제한 및 최대값 ---
  "limit_maxSpeed": 25.0,                 // 최대 속도 - 도달 가능한 최대 속도
  "limit_maxAngularSpeed": 12.0,          // 최대 회전 속도 - 도달 가능한 최대 회전 속도
  
  // --- 충돌 레이어 속성 ---
  "layer_collisionLayer": "FireBall",     // 자신의 레이어 - 물리 엔진에서 인식
  "layer_interactsWith": "Ground,Obstacles,Player,Enemy,WaterBall,GrassBall", // 충돌할 레이어들
  
  // --- 특수 능력 및 수학 연산 ---
  "special_ability": "burn",              // 특수 능력 - 화상 효과
  "special_effect": "공격한 블록을 2초간 지속 데미지", // 특수 능력 효과 설명
  "special_mathOperation": "multiply",    // 수학 연산 타입 - 곱셈 연산 사용
  "special_operationValue": 2,            // 연산 값 - 수치를 2배로 증가
  
  // --- 레벨업 및 성장 속성 ---
  "upgrade_speedGrowth": 0.5,             // 레벨당 속도 증가율
  "upgrade_damageGrowth": 2,              // 레벨당 데미지 증가율
  "upgrade_multiplierGrowth": 0.5,        // 레벨당 증폭기 증가율
  "upgrade_level6": "분열 - 벽돌 타격 시 15% 확률로 공 한 개 추가 생성", // 레벨 2 능력
  "upgrade_level9": "타오름 - 공의 데미지가 연속 타격마다 10%씩 증가 (최대 50%)", // 레벨 3 능력
  "upgrade_level12": "폭발 - 블록 파괴 시 25% 확률로 주변 블록에 50% 데미지", // 레벨 4 능력
  "upgrade_level15": "용암 흐름 - 마지막 남은 공이 떨어지면 50% 확률로 부활", // 레벨 5 능력
  
  // --- 시각 효과 ---
  "visual_prefabLabel": "FireBall",       // 프리팹 라벨 - Unity에서 사용할 모델 이름
  "visual_iconImage": "fire_ball.sprite", // 아이콘 이미지 경로
  "visual_particleEffect": "fire_trail"   // 파티클 효과 - 불 효과 트레일
}



skill -required Level 
Aoe



{
  // --- 기본 정보 ---
  "id": "basic_ball",                     // 고유 식별자
  "DataId": 201000,                       // 데이터베이스 ID
  "name": "기본 공",                      // 공의 이름
  "description": "기본적인 물리 특성을 가진 균형 잡힌 공",  // 설명
  "Rarity": "common",                     // 희귀도 (common, rare, epic, legendary 등)
  "element": "neutral",                   // 원소 속성 (neutral, fire, water 등)
  
  // --- 전투 스탯 ---
  "MaxHp": 1000.0,                        // 최대 체력
  "Atk": 25.0,                            // 기본 공격력
  "AtkBonus": 2.0,                        // 공격력 보너스 배율
  "AtkSpeed": 1.0,                        // 공격 속도 (초당 공격 횟수)
  "AtkTime": 1.0,                         // 공격 모션 시간 (초)
  "AtkRange": 4.0,                        // 공격 범위 (유닛)
  "MoveSpeed": 5.0,                       // 이동 속도 (유닛/초)
  "power": 10,                            // 공의 파워 (밀어내기 강도)
  "territory": 5,                         // 영역 범위 (유닛)
  "CriRate": 0.15,                        // 치명타 확률 (0~1)
  "CriDamage": 1.5,                       // 치명타 데미지 배율
  
  // --- Rigidbody 컴포넌트 속성 ---
  "rigidbody_mass": 1.0,                  // 질량 - 높을수록 무거움, 충돌 시 다른 물체에 더 큰 영향
  "rigidbody_drag": 0.5,                  // 선형 항력 - 공기 저항, 이동 속도 감소 정도
  "rigidbody_angularDrag": 0.05,          // 회전 항력 - 회전 속도 감소 정도
  "rigidbody_useGravity": true,           // 중력 적용 여부 - true면 중력 영향 받음
  "rigidbody_isKinematic": false,         // 키네마틱 여부 - true면 물리 영향 받지 않고 스크립트로만 제어
  "rigidbody_collisionDetectionMode": "Continuous", // 충돌 감지 모드 - 빠르게 움직이는 물체의 정확한 충돌 계산
  "rigidbody_interpolation": "None",      // 보간 방식 - 움직임 부드러움 설정
  "rigidbody_constraints": "None",        // 제약 조건 - 특정 축의 이동이나 회전 제한
  "rigidbody_maxAngularVelocity": 7.0,    // 최대 회전 속도 - 회전 속도 제한
  "rigidbody_maxDepenetrationVelocity": 10.0, // 최대 탈출 속도 - 물체가 겹쳤을 때 분리되는 속도
  
  // --- 물리 재질 속성 ---
  "physicsMaterial_bounciness": 0.7,      // 탄성 - 높을수록 잘 튕김 (0~1)
  "physicsMaterial_dynamicFriction": 0.6, // 동적 마찰 - 움직이는 물체의 마찰력
  "physicsMaterial_staticFriction": 0.6,  // 정적 마찰 - 정지 상태에서의 마찰력
  "physicsMaterial_frictionCombine": "Average", // 마찰 결합 방식 - 두 물체 마찰 계산 방법
  "physicsMaterial_bounceCombine": "Average", // 탄성 결합 방식 - 두 물체 탄성 계산 방법
  
  // --- 힘 및 속도 속성 ---
  "force_initialVelocityX": 0.0,          // 초기 속도 X - 생성 시 X축 방향 초기 속도
  "force_initialVelocityY": 0.0,          // 초기 속도 Y - 생성 시 Y축 방향 초기 속도
  "force_initialVelocityZ": 0.0,          // 초기 속도 Z - 생성 시 Z축 방향 초기 속도
  "force_initialAngularVelocityX": 0.0,   // 초기 회전 속도 X - X축 기준 회전 속도
  "force_initialAngularVelocityY": 0.0,   // 초기 회전 속도 Y - Y축 기준 회전 속도
  "force_initialAngularVelocityZ": 0.0,   // 초기 회전 속도 Z - Z축 기준 회전 속도
  "force_torqueMultiplier": 1.0,          // 회전력 계수 - 적용되는 회전력 강도 배율
  "force_forceMultiplier": 1.0,           // 힘 계수 - 적용되는 힘의 강도 배율
  "force_gravityScale": 1.0,              // 중력 스케일 - 중력 영향 정도 배율
  
  // --- 콜라이더 속성 ---
  "collider_isTrigger": false,            // 트리거 여부 - true면 물리적 충돌 없이 통과, 이벤트만 발생
  "collider_offsetX": 0.0,                // 콜라이더 X 오프셋 - X축 위치 조정
  "collider_offsetY": 0.5,                // 콜라이더 Y 오프셋 - Y축 위치 조정
  "collider_offsetZ": 0.0,                // 콜라이더 Z 오프셋 - Z축 위치 조정
  "collider_radius": 4.6,                 // 반지름 - 구체 콜라이더 크기
  "collider_physicsMaterial": "BallPhysicsMaterial", // 적용할 물리 재질 이름
  
  // --- 제한 및 최대값 ---
  "limit_maxSpeed": 20.0,                 // 최대 속도 - 공이 도달할 수 있는 최대 이동 속도
  "limit_maxAngularSpeed": 10.0,          // 최대 회전 속도 - 공이 도달할 수 있는 최대 회전 속도
  
  // --- 충돌 레이어 속성 ---
  "layer_collisionLayer": "Ball",         // 공의 레이어 - 물리 시스템에서 인식하는 레이어
  "layer_interactsWith": "Ground,Obstacles,Player,Enemy", // 충돌할 레이어들 (쉼표로 구분)
  
  // --- 기타 속성 ---
  "special": "none",                      // 특수 능력 (none, explosion, bounce 등)
  "DefaultSkillId": 10002,                // 기본 스킬 ID
  "PrefabLabel": "ServerBall",            // 프리팹 라벨
  "IconImage": "002.sprite",              // 아이콘 이미지 경로
  "ClientAvatar": "201000_Ball"           // 클라이언트 아바타 이름
}


[
  {
    "id": "fire_ball",
    "dataId": 201001,
    "name": "불공",
    "description": "빠르고 화상 데미지를 주는 공",
    "rarity": "rare",
    "element": "fire",
    "level": 1,
    "maxLevel": 5,
    
    "stat_baseSpeed": 6.5,
    "stat_baseDamage": 8,
    "stat_baseMultiplier": 2,
    "stat_maxHp": 900.0,
    "stat_atk": 35.0,
    "stat_atkBonus": 2.2,
    "stat_atkSpeed": 1.2,
    "stat_atkTime": 0.9,
    "stat_atkRange": 4.0,
    "stat_moveSpeed": 6.0,
    "stat_power": 8,
    "stat_territory": 5,
    "stat_criRate": 0.2,
    "stat_criDamage": 1.6,
    
    "rigidbody_mass": 0.7,
    "rigidbody_drag": 0.4,
    "rigidbody_angularDrag": 0.04,
    "rigidbody_useGravity": true,
    "rigidbody_isKinematic": false,
    "rigidbody_collisionDetectionMode": "Continuous",
    "rigidbody_interpolation": "Interpolate",
    "rigidbody_constraints": "None",
    "rigidbody_maxAngularVelocity": 8.0,
    "rigidbody_maxDepenetrationVelocity": 12.0,
    
    "physicsMaterial_bounciness": 0.8,
    "physicsMaterial_dynamicFriction": 0.5,
    "physicsMaterial_staticFriction": 0.5,
    "physicsMaterial_frictionCombine": "Average",
    "physicsMaterial_bounceCombine": "Maximum",
    
    "force_initialVelocityX": 0.0,
    "force_initialVelocityY": 1.0,
    "force_initialVelocityZ": 0.0,
    "force_initialAngularVelocityX": 2.0,
    "force_initialAngularVelocityY": 2.0,
    "force_initialAngularVelocityZ": 2.0,
    "force_torqueMultiplier": 1.4,
    "force_forceMultiplier": 1.3,
    "force_gravityScale": 0.95,
    "force_angleRange": 60,
    
    "collider_type": "Sphere",
    "collider_isTrigger": false,
    "collider_offsetX": 0.0,
    "collider_offsetY": 0.5,
    "collider_offsetZ": 0.0,
    "collider_radius": 0.5,
    "collider_physicsMaterial": "FireBallMaterial",
    
    "limit_maxSpeed": 25.0,
    "limit_maxAngularSpeed": 12.0,
    
    "layer_collisionLayer": "FireBall",
    "layer_interactsWith": "Ground,Obstacles,Player,Enemy,WaterBall,GrassBall",
    
    "special_ability": "burn",
    "special_effect": "공격한 블록을 2초간 지속 데미지",
    "special_mathOperation": "multiply",
    "special_operationValue": 2,
    
    "upgrade_speedGrowth": 0.5,
    "upgrade_damageGrowth": 2,
    "upgrade_multiplierGrowth": 0.5,
    "upgrade_level2": "분열 - 벽돌 타격 시 15% 확률로 공 한 개 추가 생성",
    "upgrade_level3": "타오름 - 공의 데미지가 연속 타격마다 10%씩 증가 (최대 50%)",
    "upgrade_level4": "폭발 - 블록 파괴 시 25% 확률로 주변 블록에 50% 데미지",
    "upgrade_level5": "용암 흐름 - 마지막 남은 공이 떨어지면 50% 확률로 부활",
    
    "visual_prefabLabel": "FireBall",
    "visual_iconImage": "fire_ball.sprite",
    "visual_particleEffect": "fire_trail"
  },
  
3. ball_visuals.json - 공의 시각적 효과

[
  {
    "ballId": "fireball",
    "levelVisuals": [
      {
        "level": 1,
        "renderer": {
          "type": "MeshRenderer",
          "material": {
            "shader": "Standard",
            "color": "#FF4500",
            "mainTexture": "ball_fire_1"
          }
        },
        "effects": {
          "trail": {
            "enabled": true,
            "time": 0.5,
            "startWidth": 0.3,
            "endWidth": 0.0,
            "startColor": "#FF450080",
            "endColor": "#FF000000"
          },
          "particles": {
            "enabled": true,
            "prefab": "fire_particle_1"
          },
          "light": {
            "enabled": true,
            "type": "Point",
            "color": "#FF4500",
            "intensity": 1.2,
            "range": 2.5
          }
        }
      },
      {
        "level": 5,
        "renderer": {
          "type": "MeshRenderer",
          "material": {
            "shader": "Standard",
            "color": "#FF2400",
            "mainTexture": "ball_fire_5"
          }
        },
        "effects": {
          "trail": {
            "enabled": true,
            "time": 0.7,
            "startWidth": 0.4,
            "endWidth": 0.0,
            "startColor": "#FF240080",
            "endColor": "#FF000000"
          },
          "particles": {
            "enabled": true,
            "prefab": "fire_particle_5"
          },
          "light": {
            "enabled": true,
            "type": "Point",
            "color": "#FF2400",
            "intensity": 1.6,
            "range": 3.5
          }
        }
      }
    ]
  }
]



4. ball_stats.json - 레벨별 스탯 증가
[
  {
    "ballId": "fireball",
    "levelStats": [
      {
        "level": 1,
        "statModifiers": {
          "mass": 1.0,
          "speed": 1.0,
          "bounciness": 1.0,
          "damage": 5
        }
      },
      {
        "level": 2,
        "statModifiers": {
          "mass": 1.02,
          "speed": 1.05,
          "bounciness": 1.02,
          "damage": 7
        }
      },
      {
        "level": 3,
        "statModifiers": {
          "mass": 1.05,
          "speed": 1.1,
          "bounciness": 1.05,
          "damage": 10
        }
      }
    ]
  }
]

5. skills.json - 스킬 정의
[
  {
    "id": "fire_dash",
    "name": "화염 돌진",
    "description": "짧은 거리를 불길과 함께 빠르게 돌진합니다",
    "cooldown": 5.0,
    "effectValue": 2.0,
    "effectDuration": 0.5,
    "animationPrefab": "fire_dash_effect_1",
    "soundEffect": "fire_dash_sound",
    "element": "fire",
    "skillType": "mobility"
  },
  {
    "id": "flame_trail",
    "name": "화염 흔적",
    "description": "이동 경로에 잠시 남아있는 화염 흔적을 남깁니다",
    "cooldown": 8.0,
    "effectValue": 3.0,
    "effectDuration": 2.0,
    "animationPrefab": "flame_trail_effect",
    "soundEffect": "flame_trail_sound",
    "element": "fire",
    "skillType": "persistent"
  },
  {
    "id": "frost_slow",
    "name": "냉기 감속",
    "description": "충돌한 적의 이동 속도를 감소시킵니다",
    "cooldown": 6.0,
    "effectValue": 0.5,
    "effectDuration": 3.0,
    "animationPrefab": "frost_slow_effect",
    "soundEffect": "frost_slow_sound",
    "element": "ice",
    "skillType": "debuff"
  }
]

6. ball_skill_unlocks.json - 공 레벨별 스킬 해금
[
  {
    "ballId": "fireball",
    "skillUnlocks": [
      {
        "level": 1,
        "skillId": "fire_dash"
      },
      {
        "level": 3,
        "skillId": "flame_trail"
      },
      {
        "level": 5,
        "skillId": "fire_explosion"
      },
      {
        "level": 7,
        "skillId": "heat_wave"
      },
      {
        "level": 10,
        "skillId": "inferno"
      }
    ]
  },
  {
    "ballId": "iceball",
    "skillUnlocks": [
      {
        "level": 1,
        "skillId": "frost_slow"
      },
      {
        "level": 3,
        "skillId": "ice_shield"
      },
      {
        "level": 5,
        "skillId": "freeze_area"
      },
      {
        "level": 7,
        "skillId": "ice_spike"
      },
      {
        "level": 10,
        "skillId": "blizzard"
      }
    ]
  }
]

7. ball_evolution.json - 공 진화 경로
[
  {
    "fromBallId": "fireball",
    "toBallId": "phoenix_ball",
    "requiredLevel": 10,
    "requiredItems": [
      {
        "itemId": "phoenix_feather",
        "amount": 5
      },
      {
        "itemId": "lava_stone",
        "amount": 10
      },
      {
        "itemId": "fire_essence",
        "amount": 20
      }
    ],
    "evolutionDescription": "불 공이 불사조의 힘을 얻어 피닉스 공으로 진화합니다. 자기 파괴 후 재생 능력을 얻습니다."
  },
  {
    "fromBallId": "fireball",
    "toBallId": "dragon_fire_ball",
    "requiredLevel": 10,
    "requiredItems": [
      {
        "itemId": "dragon_scale",
        "amount": 5
      },
      {
        "itemId": "volcanic_ash",
        "amount": 15
      },
      {
        "itemId": "fire_essence",
        "amount": 20
      }
    ],
    "evolutionDescription": "불 공이 용의 힘을 얻어 드래곤 파이어 공으로 진화합니다. 관통 효과와 범위 피해가 강화됩니다."
  }
]

8. minigame_performance.json - 공의 미니게임별 성능
[
  {
    "ballId": "fireball",
    "gamePerformance": [
      {
        "gameId": "pinball",
        "suitability": 8,
        "specialEffect": "플리퍼와 접촉 시 화염 궤적 남김",
        "bestUpgrade": "flame_trail"
      },
      {
        "gameId": "breakout",
        "suitability": 9,
        "specialEffect": "블록 파괴 시 주변 블록에 화염 데미지",
        "bestUpgrade": "fire_explosion"
      }
    ]
  },
  {
    "ballId": "iceball",
    "gamePerformance": [
      {
        "gameId": "pinball",
        "suitability": 7,
        "specialEffect": "충돌 지점 주변이 일시적으로 얼어붙음",
        "bestUpgrade": "freeze_area"
      },
      {
        "gameId": "breakout",
        "suitability": 8,
        "specialEffect": "블록 파괴 시 주변 블록 감속",
        "bestUpgrade": "frost_slow"
      }
    ]
  }
]


9. upgrade_requirements.json - 레벨업 요구사항
[
  {
    "ballId": "fireball",
    "upgrades": [
      {
        "fromLevel": 1,
        "toLevel": 2,
        "currency": 100,
        "materials": [
          {
            "itemId": "fire_essence",
            "amount": 2
          }
        ],
        "experiencePoints": 50
      },
      {
        "fromLevel": 2,
        "toLevel": 3,
        "currency": 250,
        "materials": [
          {
            "itemId": "fire_essence",
            "amount": 4
          }
        ],
        "experiencePoints": 150
      }
    ]
  }
]

10. set_bonuses.json - 공 세트 보너스
[
  {
    "id": "elemental_master",
    "name": "원소 마스터",
    "requiredBalls": ["fireball", "iceball", "thunderball", "earthball"],
    "bonusEffect": "모든 원소 공의 속성 효과 20% 증가",
    "bonusStats": {
      "elementalDamage": 1.2,
      "cooldownReduction": 0.1
    }
  },
  {
    "id": "fire_lord",
    "name": "불의 군주",
    "requiredBalls": ["fireball", "phoenix_ball", "dragon_fire_ball"],
    "bonusEffect": "모든 화염 계열 공의 화염 데미지 50% 증가",
    "bonusStats": {
      "fireDamage": 1.5,
      "burnDuration": 1.3
    }
  }
]


