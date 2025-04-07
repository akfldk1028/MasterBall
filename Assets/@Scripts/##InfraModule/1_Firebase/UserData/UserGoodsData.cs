// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Threading.Tasks;
// using Firebase.Firestore;

// /// <summary>
// /// 사용자 재화(골드, 젬 등) 데이터를 관리하는 클래스.
// /// Firestore의 UUserGoodsData/{PlayerId} 문서 내 'goods' 필드에 저장됩니다.
// /// </summary>
// [FirestoreData] 
// public class UserGoodsData : IUserData
// {
//     private readonly FirebaseFirestore _firestore; // Firestore 인스턴스 주입
//     private string _playerId; // UserDataManager가 설정해 줄 PlayerId
//     private readonly string _collectionPath = "UserGoodsData"; // 수정!


//     // --- Properties ---
//     public bool IsLoaded { get; set; }
//     [FirestoreProperty] public long Gold { get; set; } 
//     [FirestoreProperty] public long Gem { get; set; }
//     [FirestoreProperty] public Dictionary<string, object> BattleTickets { get; set; }
//     [FirestoreProperty] public bool SeasonTokens { get; set; }

//     // --- Constructors ---
//     public UserGoodsData() // Firestore 자동 변환용
//     {
//         SetDefaultValues();
//         IsLoaded = false;
//     }

//     // 의존성 주입용 생성자
//     public UserGoodsData(FirebaseFirestore firestore) : this()
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
//         Debug.LogWarning($"[{nameof(UserGoodsData)}] SetData(Dictionary) called unexpectedly.");
//     }
    
//     /// <summary>
//     /// UserDataManager가 호출하여 PlayerId 컨텍스트를 설정합니다.
//     /// </summary>
//     public void SetPlayerContext(string playerId)
//     {
//         if (string.IsNullOrEmpty(playerId))
//         {
//             Debug.LogError($"[{nameof(UserGoodsData)}] Invalid PlayerId provided.");
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
//                 // Firestore 자동 변환 사용 (문서 전체)
//                 UserGoodsData loadedData = snapshot.ConvertTo<UserGoodsData>();
//                 CopyFrom(loadedData); // 값 복사

//                 // 필드가 하나라도 없으면 기본값 설정
//                 if (!snapshot.ContainsField("Gold") || !snapshot.ContainsField("Gem") || 
//                     !snapshot.ContainsField("BattleTickets") || !snapshot.ContainsField("SeasonTokens"))
//                 {
//                     SetDefaultValues();
//                 }

//                 Debug.Log($"[{nameof(UserGoodsData)}] Data loaded for PlayerId: {_playerId}");
//                 IsLoaded = true;
//                 return true;
//             }
//             else
//             {
//                 Debug.LogWarning($"[{nameof(UserGoodsData)}] No goods data found for PlayerId: {_playerId}. Using default values.");
//                 SetDefaultValues();
//                 IsLoaded = true;
                
//                 // <<< 기본 데이터 저장 로직 추가 >>>
//                 try 
//                 { 
//                     await SaveDataAsync(); 
//                     Debug.Log($"[{nameof(UserGoodsData)}] Saved default goods data to Firestore for PlayerId: {_playerId}.");
//                 }
//                 catch (Exception saveEx)
//                 {
//                     Debug.LogError($"[{nameof(UserGoodsData)}] Failed to save default goods data for PlayerId: {_playerId}. Error: {saveEx.Message}");
//                 }
                
//                 return false; // 데이터가 없었음을 의미
//             }
//         }
//         catch (Exception e)
//         {
//             Debug.LogError($"[{nameof(UserGoodsData)}] Failed to load for PlayerId: {_playerId}. Error: {e.Message}");
//             SetDefaultValues();
//             IsLoaded = true;
//             return false;
//         }
//     }

//     public async Task<bool> SaveDataAsync()
//     {
//         if (!IsValidForSave()) return false;

//         try
//         {
//             DocumentReference userDocRef = _firestore.Collection(_collectionPath).Document(_playerId);
            
//             var dataToMerge = new Dictionary<string, object> 
//             {
//                 { "Gold", this.Gold },
//                 { "Gem", this.Gem },
//                 { "BattleTickets", this.BattleTickets },
//                 { "SeasonTokens", this.SeasonTokens }
//                 // 만약 Gold만 업데이트한다면: { "Gold", this.Gold }
//             };

//             // SetAsync + MergeAll 사용: 문서가 없으면 생성, 있으면 dataToMerge의 필드만 업데이트/추가
//             await userDocRef.SetAsync(dataToMerge, SetOptions.MergeAll);

//             Debug.Log($"[{nameof(UserGoodsData)}] Saved for PlayerId: {_playerId}");
//             return true;
//         }
//         catch (Exception e)
//         {
//             Debug.LogError($"[{nameof(UserGoodsData)}] Failed to save for PlayerId: {_playerId}. Error: {e.Message}");
//             return false;
//         }
//     }

//     // --- 재화 관리 메서드 (메모리만 변경) ---
//     // (AddGold, UseGold, AddGem, UseGem 등은 이전과 동일하게 유지)
//     public bool AddGold(long amount) { /* ... 이전 코드 ... */ Gold += amount; return true; }
//     public bool UseGold(long amount) { /* ... 이전 코드 ... */ if(Gold < amount) return false; Gold -= amount; return true; }
//     public bool AddGem(long amount) { /* ... 이전 코드 ... */ Gem += amount; return true; }
//     public bool UseGem(long amount) { /* ... 이전 코드 ... */ if(Gem < amount) return false; Gem -= amount; return true; }

//     #region Battle Tickets Management

//     // 티켓 충전 간격 (초 단위)
//     private const double TicketRefillIntervalSeconds = 1800; // 30분

//     /// <summary> 현재 배틀 티켓 수를 가져옵니다. </summary>
//     public long GetCurrentBattleTickets()
//     {
//         if (BattleTickets != null && BattleTickets.TryGetValue("current", out object currentObj) && currentObj is long current)
//         {
//             return current;
//         }
//         Debug.LogWarning($"[{nameof(UserGoodsData)}] Could not retrieve 'current' battle tickets.");
//         return 0;
//     }

//     /// <summary> 최대 배틀 티켓 수를 가져옵니다. </summary>
//     public long GetMaxBattleTickets()
//     {
//         if (BattleTickets != null && BattleTickets.TryGetValue("maximum", out object maxObj) && maxObj is long max)
//         {
//             return max;
//         }
//         Debug.LogWarning($"[{nameof(UserGoodsData)}] Could not retrieve 'maximum' battle tickets.");
//         // 기본값 또는 설정값을 반환해야 할 수 있음
//         return 5; // 예시 기본값
//     }

//     /// <summary> 배틀 티켓 1개를 사용합니다. 성공 시 true를 반환합니다. </summary>
//     public bool UseBattleTicket()
//     {
//         if (BattleTickets == null || !BattleTickets.ContainsKey("current"))
//         {
//             Debug.LogError($"[{nameof(UserGoodsData)}] BattleTickets dictionary is not properly initialized or 'current' key is missing.");
//             return false;
//         }

//         if (BattleTickets["current"] is long currentTickets && currentTickets > 0)
//         {
//             // 티켓 사용 시, 만약 티켓이 최대치였다면 다음 리필 시간 설정
//             if (currentTickets == GetMaxBattleTickets())
//             {
//                 BattleTickets["nextRefillTime"] = Timestamp.FromDateTime(DateTime.UtcNow.AddSeconds(TicketRefillIntervalSeconds));
//             }

//             BattleTickets["current"] = currentTickets - 1;
//             Debug.Log($"[{nameof(UserGoodsData)}] Battle ticket used. Remaining: {BattleTickets["current"]}");
//             return true;
//         }
//         else
//         {
//             Debug.LogWarning($"[{nameof(UserGoodsData)}] Not enough battle tickets or invalid data type. Current: {BattleTickets["current"]}");
//             return false;
//         }
//     }

//     /// <summary> 배틀 티켓을 지정된 양만큼 추가합니다. 최대치를 초과하지 않습니다. </summary>
//     public void AddBattleTickets(int amountToAdd)
//     {
//         if (amountToAdd <= 0) return;
//         if (BattleTickets == null || !BattleTickets.ContainsKey("current") || !BattleTickets.ContainsKey("maximum"))
//         {
//             Debug.LogError($"[{nameof(UserGoodsData)}] BattleTickets dictionary is not properly initialized or keys are missing.");
//             return;
//         }

//         if (BattleTickets["current"] is long currentTickets && BattleTickets["maximum"] is long maxTickets)
//         {
//             long newAmount = Math.Min(currentTickets + amountToAdd, maxTickets);
//             BattleTickets["current"] = newAmount;
//             Debug.Log($"[{nameof(UserGoodsData)}] Added {amountToAdd} battle tickets. Current: {newAmount}");
//         }
//         else
//         {
//             Debug.LogError($"[{nameof(UserGoodsData)}] Invalid data types in BattleTickets dictionary.");
//         }
//     }

//     /// <summary> 배틀 티켓 리필 로직 </summary>
//     public void CheckAndRefillBattleTickets()
//     {
//         if (BattleTickets == null || !BattleTickets.ContainsKey("current") || !BattleTickets.ContainsKey("maximum") || !BattleTickets.ContainsKey("nextRefillTime"))
//         {
//             Debug.LogError($"[{nameof(UserGoodsData)}] BattleTickets dictionary is not properly initialized or keys are missing for refill check.");
//             return;
//         }

//         if (!(BattleTickets["current"] is long currentTickets) || !(BattleTickets["maximum"] is long maxTickets))
//         {
//             Debug.LogError($"[{nameof(UserGoodsData)}] Invalid data types in BattleTickets dictionary for refill check.");
//             return;
//         }

//         // 이미 최대치라면 리필할 필요 없음
//         if (currentTickets >= maxTickets)
//         {
//             // nextRefillTime을 미래의 특정 시간이나 null/sentinel 값으로 설정하여 불필요한 계산 방지 가능
//             // 여기서는 단순히 리턴
//             return;
//         }

//         if (!(BattleTickets["nextRefillTime"] is Timestamp nextRefillTimestamp))
//         {
//             Debug.LogError($"[{nameof(UserGoodsData)}] Invalid 'nextRefillTime' data type in BattleTickets dictionary.");
//             // 기본값 또는 오류 복구 로직 필요 시 추가
//             // 예: BattleTickets["nextRefillTime"] = Timestamp.FromDateTime(DateTime.UtcNow.AddSeconds(TicketRefillIntervalSeconds));
//             return;
//         }

//         DateTime nextRefillTime = nextRefillTimestamp.ToDateTime();
//         DateTime currentTime = DateTime.UtcNow;

//         // 아직 리필 시간이 안 됐으면 종료
//         if (currentTime < nextRefillTime)
//         {
//             return;
//         }

//         // --- 리필 로직 시작 ---
//         TimeSpan timePassed = currentTime - nextRefillTime;
//         int ticketsToRefill = (int)(timePassed.TotalSeconds / TicketRefillIntervalSeconds) + 1; // 경과 시간 동안 충전됐어야 할 티켓 수

//         long newTicketCount = currentTickets + ticketsToRefill;
//         long actualTicketsToAdd = 0;

//         if (newTicketCount >= maxTickets)
//         {
//             actualTicketsToAdd = maxTickets - currentTickets;
//             BattleTickets["current"] = maxTickets;
//             // 최대치 도달 시 다음 리필 시간은 의미 없음 (또는 미래 시간으로 설정)
//              BattleTickets["nextRefillTime"] = Timestamp.FromDateTime(DateTime.UtcNow.AddYears(1)); // 예: 1년 뒤로 설정
//         }
//         else
//         {
//             actualTicketsToAdd = ticketsToRefill;
//             BattleTickets["current"] = newTicketCount;
//             // 다음 리필 시간 계산: 마지막 리필 시간 + (충전된 티켓 수 * 간격)
//             DateTime newNextRefillTime = nextRefillTime.AddSeconds(ticketsToRefill * TicketRefillIntervalSeconds);
//             BattleTickets["nextRefillTime"] = Timestamp.FromDateTime(newNextRefillTime);
//         }

//         if (actualTicketsToAdd > 0)
//         {
//              Debug.Log($"[{nameof(UserGoodsData)}] Refilled {actualTicketsToAdd} battle tickets. Current: {BattleTickets["current"]}. Next refill at: {BattleTickets["nextRefillTime"]}");
//         }
//     }

//     #endregion

//     #region Season Tokens Management

//     /// <summary> 시즌 토큰을 부여합니다. </summary>
//     public void GrantSeasonToken()
//     {
//         if (!SeasonTokens)
//         {
//             SeasonTokens = true;
//             Debug.Log($"[{nameof(UserGoodsData)}] Season token granted.");
//         }
//     }

//     /// <summary> 시즌 토큰을 소모합니다. </summary>
//     public void ConsumeSeasonToken()
//     {
//         if (SeasonTokens)
//         {
//             SeasonTokens = false;
//             Debug.Log($"[{nameof(UserGoodsData)}] Season token consumed.");
//         }
//     }

//     #endregion

//     public bool HasMappedAccount() => false; // UserGoodsData는 계정 매핑 정보 없음

//     // --- Private Helpers ---
    
//     /// <summary> 다른 UserGoodsData 인스턴스에서 현재 인스턴스로 값을 복사합니다. </summary>
//     private void CopyFrom(UserGoodsData source)
//     {
//         if (source == null) return;

//         this.Gold = source.Gold;
//         this.Gem = source.Gem;
//         this.BattleTickets = source.BattleTickets;
//         this.SeasonTokens = source.SeasonTokens;
//     }
    
//     private void SetDefaultValues()
//     {
//         Gold = 100; // 기본 골드 (예시)
//         Gem = 10;  // 기본 젬 (예시)
//         BattleTickets = new Dictionary<string, object>
//         {
//             { "current", 5 },       // 예: 기본 5개
//             { "maximum", 5 },       // 예: 최대 5개
//             { "nextRefillTime", FieldValue.ServerTimestamp } // 수정: Timestamp.FromServerTimestamp -> FieldValue.ServerTimestamp
//         };
//         SeasonTokens = false; // 예: 기본값은 false
//     }

//     private bool IsValidForLoad()
//     {
//         if (_firestore == null) { Debug.LogError("Firestore instance is null."); return false; }
//         if (string.IsNullOrEmpty(_playerId)) { Debug.LogError("PlayerId is not set."); return false; }
//         return true;
//     }
    
//     private bool IsValidForSave()
//     {        
//         if (_firestore == null) { Debug.LogError("Firestore instance is null."); return false; }
//         if (string.IsNullOrEmpty(_playerId)) { Debug.LogError("PlayerId is not set."); return false; }
//         return true;
//     }

//     private void LogTaskCompletion(Task task)
//     {
//         if (task.IsFaulted)
//         {            
//             Debug.LogError($"[{nameof(UserGoodsData)}] Task failed for PlayerId: {_playerId ?? "(Unknown)"}. Error: {task.Exception}");
//         }
//     }
// }
