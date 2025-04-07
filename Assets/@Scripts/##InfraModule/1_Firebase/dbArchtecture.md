DeviceMappingData
├── GoogleUID           // 플레이어 닉네임
├── AppleUID: number              // 플레이어 레벨
├── PlayerId: number         // 현재 경험치
├── joinDate: timestamp        // 가입 일자
├── lastLoginAt: timestamp     // 마지막 로그인 시간
└── status: string             // 현재 상태 (online, offline, in-game)



profile (맵)
├── nickname: string           // 플레이어 닉네임
├── level: number              // 플레이어 레벨
├── experience: number         // 현재 경험치
├── experienceToNextLevel: number      // 다음 레벨까지 필요한 경험치
├── rank: string               // 플레이어 랭크 (Bronze, Silver, Gold 등)
├── rankPoints: number         // 랭크 포인트
├── avatarId: string           // 프로필 아바타 ID
├── title: string              // 칭호 (있을 경우)
├── joinDate: timestamp        // 가입 일자
├── lastLoginAt: timestamp     // 마지막 로그인 시간



resources (맵)
├── gold: number              // 기본 재화
├── gems: number              // 프리미엄 재화
├── battleTickets: {          // 대전 티켓
│   ├── current: number      // 현재 티켓 수
│   ├── maximum: number      // 최대 티켓 수
│   └── nextRefillTime: timestamp // 다음 티켓 회복 시간
│   }
└── seasonTokens: bool 

settings (맵)
├── musicVolume: number        // 배경 음악 볼륨 (0.0 ~ 1.0)
├── sfxVolume: number          // 효과음 볼륨 (0.0 ~ 1.0)
├── notificationsEnabled: boolean // 푸시 알림 활성화 여부
├── language: string           // 선택한 언어
├── graphicsQuality: string    // 그래픽 품질
├── autoMatchmaking: boolean   // 자동 매치메이킹 활성화 여부
└── preferredRegion: string    // 선호 서버 지역




achievements (맵)
├── achievement_monster_hunter: {
│   ├── id: string           // 업적 ID
│   ├── progress: number     // 현재 진행도
│   ├── target: number       // 목표치
│   ├── completed: boolean   // 완료 여부
│   └── claimedReward: boolean // 보상 수령 여부
│   }
│
├── achievement_team_player: { ... },
└── ... (기타 업적)


gameProgress (맵)
├── playerRating: number       // 매치메이킹용 플레이어 점수
├── highestDifficulty: number  // 클리어한 최고 난이도
├── totalGamesPlayed: number   // 총 게임 플레이  수
├── totalWins: number          // 총 승리 수
├── totalLosses: number        // 총 패배 수
├── monstersDefeated: number   // 처치한 몬스터 수
├── wavesSurvived: number      // 생존한 웨이브 수
├── bestSurvivalWave: number   // 최고 생존 웨이브
└── lastGameResult: {          // 마지막 게임 결과
    ├── gameId: string        // 게임 세션 ID
    ├── difficulty: number    // 난이도
    ├── result: string        // win/lose
    ├── wavesCompleted: number // 완료한 웨이브 수
    ├── teamMembers: [string, string, ...] // 팀원 ID 목록
    └── timestamp: timestamp  // 게임 종료 시간
    }



FullGameProgress (맵)
├── currentLevel: number               // 플레이어의 현재 레벨
├── experience: number                 // 현재 경험치
├── experienceToNextLevel: number      // 다음 레벨까지 필요한 경험치
│── totalWins: number          // 총 승리 수
├── totalLosses: number        // 총 패배 
── gamesPlayed: number            // 총 게임 플레이 수
│   ├── winningstreak: number            // 달성한 최고 연승
│   ├── totalGoldEarned: number        // 총 획득 골드
│   └── totalGemsEarned: number        // 총 획득 젬


├── basicgamestats: {                           // 누적 통계 
│   ├── gamesPlayed: number            // 총 게임 플레이 수
│   ├── wins: number                   // 총 승리 수
│   ├── losses: number                 // 총 패배 수
│   ├── highestWave: number            // 달성한 최고 연승
│   ├── totalPlayTime: number          // 총 플레이 시간(분)
│   ├── totalGoldEarned: number        // 총 획득 골드
│   └── totalGemsEarned: number        // 총 획득 젬
│   }

├── personalBests: {                   // 개인 최고 기록
│   ├── bestWaveByMap: {               // 맵별 최고 웨이브
│   │   ├── map_1: number
│   │   ├── map_2: number
│   │   └── ... (기타 맵)
│   │   }
│   ├── bestScoreSingle: number        // 솔로 플레이 최고 점수
│   ├── bestScoreTeam: number          // 팀 플레이 최고 점수
│   └── fastestClearTimes: {           // 최단 클리어 시간(초)
│       ├── easy: number
│       ├── normal: number
│       ├── hard: number
│       └── expert: number
│       }
│   }
│
├── currentSeason: {                   // 현재 시즌 정보
│   ├── seasonId: number               // 시즌 ID
│   ├── seasonRank: string             // 시즌 랭크
│   ├── seasonPoints: number           // 시즌 포인트
│   ├── seasonRewardsCollected: [number, ...] // 수령한 시즌 보상 레벨
│   └── placement: {                   // 랭킹전 배치 정보
│       ├── matchesPlayed: number      // 플레이한 배치 경기 수
│       ├── matchesRequired: number    // 필요한 배치 경기 수
│       └── provisionalRating: number  // 임시 레이팅
│       }
│   }
│
