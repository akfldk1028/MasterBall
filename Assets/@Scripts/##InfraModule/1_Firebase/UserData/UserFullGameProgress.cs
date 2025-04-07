// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Threading.Tasks;
// using Firebase.Firestore;
// using System.Linq; // For results.All() in LoadUserDataAsync

// /// <summary>
// /// 플레이어의 전체 게임 진행 상황 데이터를 관리하는 클래스.
// /// Firestore의 UserFullGameProgress/{PlayerId} 문서에 저장됩니다.
// /// </summary>
// [FirestoreData]
// public class UserFullGameProgress : IUserData
// {
//     private readonly FirebaseFirestore _firestore;
//     private string _playerId;
//     private readonly string _collectionPath = "UserFullGameProgress"; // 컬렉션 이름

//     // --- Properties ---
//     public bool IsLoaded { get; set; }

//     [FirestoreProperty] public long TotalWins { get; set; }
//     [FirestoreProperty] public long TotalLosses { get; set; }
//     [FirestoreProperty] public long GamesPlayed { get; set; }
//     [FirestoreProperty] public long WinningStreak { get; set; } // 최고 연승 기록으로 가정
//     [FirestoreProperty] public long TotalGoldEarned { get; set; }
//     [FirestoreProperty] public long TotalGemsEarned { get; set; }

//     // --- Constructors ---
//     public UserFullGameProgress() // Firestore 자동 변환용
//     {
//         SetDefaultValues();
//         IsLoaded = false;
//     }

//     // 의존성 주입용 생성자
//     public UserFullGameProgress(FirebaseFirestore firestore) : this()
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
//         Debug.LogWarning($"[{nameof(UserFullGameProgress)}] SetData(Dictionary) called unexpectedly.");
//     }

//     /// <summary>
//     /// UserDataManager가 호출하여 PlayerId 컨텍스트를 설정합니다.
//     /// </summary>
//     public void SetPlayerContext(string playerId)
//     {
//         if (string.IsNullOrEmpty(playerId))
//         {
//             Debug.LogError($"[{nameof(UserFullGameProgress)}] Invalid PlayerId provided.");
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
//                 UserFullGameProgress loadedData = snapshot.ConvertTo<UserFullGameProgress>();
//                 CopyFrom(loadedData); // 값 복사

//                 // 필드가 하나라도 없으면 기본값 설정 (삭제된 필드 제외)
//                 if (!snapshot.ContainsField("TotalWins") || !snapshot.ContainsField("TotalLosses") ||
//                     !snapshot.ContainsField("GamesPlayed") || !snapshot.ContainsField("WinningStreak") ||
//                     !snapshot.ContainsField("TotalGoldEarned") || !snapshot.ContainsField("TotalGemsEarned"))
//                 {
//                      Debug.LogWarning($"[{nameof(UserFullGameProgress)}] Some fields missing for PlayerId: {_playerId}. Applying default values.");
//                      SetDefaultValues();
//                 }

//                 Debug.Log($"[{nameof(UserFullGameProgress)}] Data loaded for PlayerId: {_playerId}");
//                 IsLoaded = true;
//                 return true;
//             }
//             else
//             {
//                 Debug.LogWarning($"[{nameof(UserFullGameProgress)}] No progress data found for PlayerId: {_playerId}. Using default values.");
//                 SetDefaultValues(); // 새 유저이므로 기본값 설정
//                 IsLoaded = true;

//                 // 새 유저의 기본 데이터 저장
//                 try
//                 {
//                     await SaveDataAsync();
//                     Debug.Log($"[{nameof(UserFullGameProgress)}] Saved default progress data to Firestore for PlayerId: {_playerId}.");
//                     return true; // 기본값 로드 성공 (원래 데이터는 없었음)
//                 }
//                 catch (Exception saveEx)
//                 {
//                     Debug.LogError($"[{nameof(UserFullGameProgress)}] Failed to save default progress data for PlayerId: {_playerId}. Error: {saveEx.Message}");
//                     return false; // 저장 실패
//                 }
//             }
//         }
//         catch (Exception e)
//         {
//             Debug.LogError($"[{nameof(UserFullGameProgress)}] Failed to load for PlayerId: {_playerId}. Error: {e.Message}");
//             // 로드 실패 시 기본값 유지 또는 다른 처리
//             SetDefaultValues();
//             IsLoaded = false; // 로드 실패 명시
//             return false;
//         }
//     }

//     public async Task<bool> SaveDataAsync()
//     {
//         if (!IsValidForSave()) return false;

//         try
//         {
//             DocumentReference userDocRef = _firestore.Collection(_collectionPath).Document(_playerId);

//             // 모든 속성을 저장 (FirestoreData 속성 사용)
//             await userDocRef.SetAsync(this, SetOptions.MergeAll);

//             Debug.Log($"[{nameof(UserFullGameProgress)}] Saved for PlayerId: {_playerId}");
//             return true;
//         }
//         catch (Exception e)
//         {
//             Debug.LogError($"[{nameof(UserFullGameProgress)}] Failed to save for PlayerId: {_playerId}. Error: {e.Message}");
//             return false;
//         }
//     }

//     // --- 게임 진행 데이터 관리 메서드 (예시) ---
//     public void RecordGameResult(bool win, long goldEarned, long gemsEarned)
//     {
//         GamesPlayed++;
//         TotalGoldEarned += goldEarned;
//         TotalGemsEarned += gemsEarned;

//         if (win)
//         {
//             TotalWins++;
//             // 연승 로직
//         }
//         else
//         {
//             TotalLosses++;
//             // 연승 초기화 로직
//         }

//         // 변경된 데이터 저장 요청
//         // SaveData();
//     }

//     public bool HasMappedAccount() => false;

//     // --- Private Helpers ---

//     /// <summary> 다른 UserFullGameProgress 인스턴스에서 현재 인스턴스로 값을 복사합니다. </summary>
//     private void CopyFrom(UserFullGameProgress source)
//     {
//         if (source == null) return;

//         this.TotalWins = source.TotalWins;
//         this.TotalLosses = source.TotalLosses;
//         this.GamesPlayed = source.GamesPlayed;
//         this.WinningStreak = source.WinningStreak;
//         this.TotalGoldEarned = source.TotalGoldEarned;
//         this.TotalGemsEarned = source.TotalGemsEarned;
//         // IsLoaded, _firestore, _playerId 등은 복사하지 않음
//     }

//     private void SetDefaultValues()
//     {
//         TotalWins = 0;
//         TotalLosses = 0;
//         GamesPlayed = 0;
//         WinningStreak = 0;
//         TotalGoldEarned = 0;
//         TotalGemsEarned = 0;
//     }

//     private bool IsValidForLoad()
//     {
//         if (_firestore == null) { Debug.LogError($"[{nameof(UserFullGameProgress)}] Firestore instance is null."); return false; }
//         if (string.IsNullOrEmpty(_playerId)) { Debug.LogError($"[{nameof(UserFullGameProgress)}] PlayerId is not set."); return false; }
//         return true;
//     }

//     private bool IsValidForSave()
//     {
//         if (_firestore == null) { Debug.LogError($"[{nameof(UserFullGameProgress)}] Firestore instance is null."); return false; }
//         if (string.IsNullOrEmpty(_playerId)) { Debug.LogError($"[{nameof(UserFullGameProgress)}] PlayerId is not set."); return false; }
//         return true;
//     }

//     private void LogTaskCompletion(Task task)
//     {
//         if (task.IsFaulted)
//         {
//             Debug.LogError($"[{nameof(UserFullGameProgress)}] Task failed for PlayerId: {_playerId ?? "(Unknown)"}. Error: {task.Exception}");
//         }
//     }
// }
