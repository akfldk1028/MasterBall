// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Threading.Tasks;
// using Firebase.Firestore;
// using System.Linq;

// /// <summary>
// /// 플레이어 프로필 데이터를 관리하는 클래스.
// /// Firestore의 UserProfile/{PlayerId} 문서에 저장됩니다.
// /// </summary>
// [FirestoreData]
// public class UserProfile : IUserData
// {
//     private readonly FirebaseFirestore _firestore;
//     private string _playerId;
//     private readonly string _collectionPath = "UserProfile"; // 컬렉션 이름

//     // --- Properties ---
//     public bool IsLoaded { get; set; }

//     [FirestoreProperty] public string Nickname { get; set; }
//     [FirestoreProperty] public int Level { get; set; }
//     [FirestoreProperty] public long Experience { get; set; }
//     [FirestoreProperty] public long ExperienceToNextLevel { get; set; }
//     [FirestoreProperty] public string Rank { get; set; }
//     [FirestoreProperty] public int RankPoints { get; set; }
//     [FirestoreProperty] public string AvatarId { get; set; }
//     [FirestoreProperty] public string Title { get; set; }
//     [FirestoreProperty] public Timestamp JoinDate { get; set; }
//     [FirestoreProperty] public Timestamp LastLoginAt { get; set; }

//     // --- Constructors ---
//     public UserProfile() // Firestore 자동 변환용
//     {
//         SetDefaultValues();
//         IsLoaded = false;
//     }

//     // 의존성 주입용 생성자
//     public UserProfile(FirebaseFirestore firestore) : this()
//     {
//         _firestore = firestore ?? throw new ArgumentNullException(nameof(firestore));
//     }

//     // --- IUserData Implementation & Context Setting ---
//     public void SetDefaultData() => SetDefaultValues();
//     public void LoadData() => LoadDataAsync().ContinueWith(LogTaskCompletion);
//     public void SaveData() => SaveDataAsync().ContinueWith(LogTaskCompletion);

//     // IUserData 인터페이스 요구사항 (자동 매핑 사용 시 불필요)
//     public void SetData(Dictionary<string, object> data)
//     {
//         Debug.LogWarning($"[{nameof(UserProfile)}] SetData(Dictionary) called unexpectedly.");
//     }

//     /// <summary>
//     /// UserDataManager가 호출하여 PlayerId 컨텍스트를 설정합니다.
//     /// </summary>
//     public void SetPlayerContext(string playerId)
//     {
//         if (string.IsNullOrEmpty(playerId))
//         {
//             Debug.LogError($"[{nameof(UserProfile)}] Invalid PlayerId provided.");
//             return;
//         }
//         _playerId = playerId;
//     }

//     // --- Async Methods ---
//     public async Task<bool> LoadDataAsync()
//     {
//         if (!IsValidForLoad()) return false;

//         IsLoaded = false;
//         try
//         {
//             DocumentReference docRef = _firestore.Collection(_collectionPath).Document(_playerId);
//             DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

//             if (snapshot.Exists)
//             {
//                 // Firestore 자동 변환 사용
//                 UserProfile loadedData = snapshot.ConvertTo<UserProfile>();
//                 CopyFrom(loadedData); // 값 복사

//                 // 필드가 하나라도 없으면 기본값 설정
//                 if (!snapshot.ContainsField("Nickname") || !snapshot.ContainsField("Level") ||
//                     !snapshot.ContainsField("Experience") || !snapshot.ContainsField("ExperienceToNextLevel") ||
//                     !snapshot.ContainsField("Rank") || !snapshot.ContainsField("RankPoints") ||
//                     !snapshot.ContainsField("AvatarId") || !snapshot.ContainsField("Title") ||
//                     !snapshot.ContainsField("JoinDate") || !snapshot.ContainsField("LastLoginAt"))
//                 {
//                     Debug.LogWarning($"[{nameof(UserProfile)}] Some fields missing for PlayerId: {_playerId}. Applying default values.");
//                     SetDefaultValues();
//                 }


//                 Debug.Log($"[{nameof(UserProfile)}] Data loaded for PlayerId: {_playerId}");
//                 IsLoaded = true;
//                 return true;
//             }
//             else
//             {
//                 Debug.LogWarning($"[{nameof(UserProfile)}] No profile data found for PlayerId: {_playerId}. Using default values.");
//                 SetDefaultValues(); // 새 유저이므로 기본값 설정
//                 IsLoaded = true;

//                 // 새 유저의 기본 데이터 저장
//                 try
//                 {
//                     await SaveDataAsync();
//                     Debug.Log($"[{nameof(UserProfile)}] Saved default profile data to Firestore for PlayerId: {_playerId}.");
//                     return true;
//                 }
//                 catch (Exception saveEx)
//                 {
//                     Debug.LogError($"[{nameof(UserProfile)}] Failed to save default profile data for PlayerId: {_playerId}. Error: {saveEx.Message}");
//                     return false;
//                 }
//             }
//         }
//         catch (Exception e)
//         {
//             Debug.LogError($"[{nameof(UserProfile)}] Failed to load for PlayerId: {_playerId}. Error: {e.Message}");
//             SetDefaultValues();
//             IsLoaded = false;
//             return false;
//         }
//     }

//     public async Task<bool> SaveDataAsync()
//     {
//         if (!IsValidForSave()) return false;

//         try
//         {
//             DocumentReference userDocRef = _firestore.Collection(_collectionPath).Document(_playerId);
//             // 마지막 로그인 시간 갱신
//             this.LastLoginAt = Timestamp.GetCurrentTimestamp();
//             await userDocRef.SetAsync(this, SetOptions.MergeAll);

//             Debug.Log($"[{nameof(UserProfile)}] Saved for PlayerId: {_playerId}");
//             return true;
//         }
//         catch (Exception e)
//         {
//             Debug.LogError($"[{nameof(UserProfile)}] Failed to save for PlayerId: {_playerId}. Error: {e.Message}");
//             return false;
//         }
//     }

//     // --- 프로필 데이터 관리 메서드 ---
//     public void UpdateNickname(string newNickname)
//     {
//         if (!string.IsNullOrWhiteSpace(newNickname))
//         {
//             Nickname = newNickname;
//             // 변경 사항 저장 요청
//             // SaveData();
//         }
//     }

//     public void UpdateAvatar(string newAvatarId)
//     {
//         if (!string.IsNullOrWhiteSpace(newAvatarId))
//         {
//             AvatarId = newAvatarId;
//             // 변경 사항 저장 요청
//             // SaveData();
//         }
//     }

//     public void AddExperience(long amount)
//     {
//         if (amount <= 0) return;

//         Experience += amount;
//         CheckForLevelUp();
//         // 변경 사항 저장 요청
//         // SaveData();
//     }

//     private void CheckForLevelUp()
//     {
//         // 간단한 레벨업 로직 예시
//         while (Experience >= ExperienceToNextLevel && Level < 100) // 최대 레벨 가정
//         {
//             Experience -= ExperienceToNextLevel;
//             Level++;
//             // 다음 레벨 필요 경험치 계산 (예: 더 복잡한 공식 사용 가능)
//             ExperienceToNextLevel = CalculateExperienceForNextLevel(Level);
//             Debug.Log($"[{nameof(UserProfile)}] Level Up! Reached Level: {Level}");
//             // 레벨업 이벤트 발생 등 추가 로직
//         }
//     }

//     private long CalculateExperienceForNextLevel(int level)
//     {
//         // 예시: 레벨별 필요 경험치 증가
//         return 100 + (level * 50);
//     }

//     public void UpdateRankDetails(string rank, int rankPoints)
//     {
//         Rank = rank;
//         RankPoints = rankPoints;
//         // 변경 사항 저장 요청
//         // SaveData();
//     }

//     public void SetTitle(string title)
//     {
//         if (!string.IsNullOrWhiteSpace(title))
//         {
//             Title = title;
//             // 변경 사항 저장 요청
//             // SaveData();
//         }
//     }

//     public bool HasMappedAccount() => false;

//     // --- Private Helpers ---
//     private void CopyFrom(UserProfile source)
//     {
//         if (source == null) return;

//         this.Nickname = source.Nickname;
//         this.Level = source.Level;
//         this.Experience = source.Experience;
//         this.ExperienceToNextLevel = source.ExperienceToNextLevel;
//         this.Rank = source.Rank;
//         this.RankPoints = source.RankPoints;
//         this.AvatarId = source.AvatarId;
//         this.Title = source.Title;
//         this.JoinDate = source.JoinDate;
//         this.LastLoginAt = source.LastLoginAt;
//     }

//     private void SetDefaultValues()
//     {
//         Nickname = $"Player_{Guid.NewGuid().ToString().Substring(0, 6)}"; // 임시 닉네임
//         Level = 1;
//         Experience = 0;
//         ExperienceToNextLevel = CalculateExperienceForNextLevel(Level); // 초기 필요 경험치
//         Rank = "Bronze";
//         RankPoints = 0;
//         AvatarId = "default_avatar"; // 기본 아바타 ID
//         Title = ""; // 기본 칭호 없음
//         JoinDate = Timestamp.GetCurrentTimestamp(); // 가입 시점
//         LastLoginAt = Timestamp.GetCurrentTimestamp(); // 마지막 로그인(초기화 시점)
//     }

//     private bool IsValidForLoad()
//     {
//         if (_firestore == null) { Debug.LogError($"[{nameof(UserProfile)}] Firestore instance is null."); return false; }
//         if (string.IsNullOrEmpty(_playerId)) { Debug.LogError($"[{nameof(UserProfile)}] PlayerId is not set."); return false; }
//         return true;
//     }

//     private bool IsValidForSave()
//     {
//         if (_firestore == null) { Debug.LogError($"[{nameof(UserProfile)}] Firestore instance is null."); return false; }
//         if (string.IsNullOrEmpty(_playerId)) { Debug.LogError($"[{nameof(UserProfile)}] PlayerId is not set."); return false; }
//         return true;
//     }

//     private void LogTaskCompletion(Task task)
//     {
//         if (task.IsFaulted)
//         {
//             Debug.LogError($"[{nameof(UserProfile)}] Task failed for PlayerId: {_playerId ?? "(Unknown)"}. Error: {task.Exception}");
//         }
//     }
// }
