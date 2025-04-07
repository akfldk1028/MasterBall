// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Firebase.Firestore;
// using UnityEngine;

// /// <summary>
// /// Firestore에 디바이스 ID와 사용자 정보를 매핑하여 관리하는 클래스
// /// </summary>
// [FirestoreData] // Firestore 자동 매핑 활성화
// public class DeviceMappingData : IUserData
// {
//     private readonly FirebaseFirestore _firestore;
//     private readonly string _collectionPath = "DeviceMappingData";

//     // --- Properties ---
//     // IsLoaded는 Firestore에 저장되지 않는 상태값
//     public bool IsLoaded { get; set; }
    
//     // DeviceId는 문서 ID로 사용되므로 FirestoreProperty 아님
//     public string DeviceId { get; private set; }
    
//     // Firestore 필드로 자동 매핑될 속성들
//     [FirestoreProperty] public string GoogleUID { get; set; }
//     [FirestoreProperty] public string AppleUID { get; set; }
//     [FirestoreProperty] public string PlayerId { get; set; }
//     [FirestoreProperty] public string Nickname { get; set; }
//     [FirestoreProperty] public DateTime LastLoginTime { get; set; }
//     [FirestoreProperty] public bool IsGuestAccount { get; set; }

//     // --- Constructors ---
//     // Firestore 자동 변환을 위한 기본 생성자
//     public DeviceMappingData()
//     {
//         SetDefaultValues();
//         IsLoaded = false;
//     }

//     // 의존성 주입용 생성자 (UserDataManager에서 사용)
//     public DeviceMappingData(FirebaseFirestore firestore) : this() // 기본 생성자 호출
//     {
//         _firestore = firestore ?? throw new ArgumentNullException(nameof(firestore));
//         DeviceId = SystemInfo.deviceUniqueIdentifier;
//     }

//     // --- IUserData Implementation ---
//     public void SetDefaultData() => SetDefaultValues();

//     public void LoadData() => LoadDataAsync().ContinueWith(LogTaskCompletion);
    
//     public void SaveData() => SaveDataAsync().ContinueWith(LogTaskCompletion);

//     // IUserData 인터페이스 요구사항이나, FirestoreData 자동 매핑 사용 시 직접 필요하지 않음
//     public void SetData(Dictionary<string, object> data)
//     {
//         Debug.LogWarning($"[DeviceMappingData] SetData(Dictionary) called but FirestoreData attribute is used for automatic mapping.");
//     }

//     // --- Async Methods ---
//     public async Task<bool> LoadDataAsync()
//     {
//         IsLoaded = false;
//         if (!IsValidState()) return false;

//         try
//         {
//             DocumentReference docRef = _firestore.Collection(_collectionPath).Document(DeviceId);
//             DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

//             if (snapshot.Exists)
//             {
//                 DeviceMappingData loadedData = snapshot.ConvertTo<DeviceMappingData>();
//                 CopyFrom(loadedData); // 값 복사
//                 IsLoaded = true;
//                 return true;
//             }
//             else
//             {
//                 Debug.Log($"[{nameof(DeviceMappingData)}] No document found for DeviceId: {DeviceId}. Applying default data.");
//                 SetDefaultData(); // 기본값 적용
//                 IsLoaded = true;  // 로드 시도는 완료됨
    
//                 try 
//                 { 
//                     await SaveDataAsync(); 
//                     Debug.Log($"[{nameof(DeviceMappingData)}] Saved default data to Firestore for DeviceId: {DeviceId}.");
//                 }
//                 catch (Exception saveEx)
//                 {
//                     Debug.LogError($"[{nameof(DeviceMappingData)}] Failed to save default data for DeviceId: {DeviceId}. Error: {saveEx.Message}");
//                     // 기본 데이터 저장 실패 시 별도 처리 필요시 추가
//                 }
//                 // <<< 로직 추가 끝 >>>
                
//                 return false; // 데이터가 없었음을 의미
//             }
//         }
//         catch (Exception e)
//         {
//             Debug.LogError($"[DeviceMappingData] Failed to load data: {e.Message}");
//             SetDefaultValues();
//             IsLoaded = true;
//             return false;
//         }
//     }

//     public async Task<bool> SaveDataAsync()
//     {
//         if (!IsValidState()) return false;

//         try
//         {
//             DocumentReference docRef = _firestore.Collection(_collectionPath).Document(DeviceId);

//             await docRef.SetAsync(this, SetOptions.MergeAll);

//             Debug.Log($"[{nameof(DeviceMappingData)}] Data saved successfully for DeviceId: {DeviceId}.");
//             return true;
//         }
//         catch (Exception e)
//         {
//             Debug.LogError($"[DeviceMappingData] Failed to save data: {e.Message}");
//             return false;
//         }
//     }

//     // --- Utility Methods ---
//     public void MapToGoogle(string googleUID)
//     {
//         GoogleUID = googleUID ?? string.Empty;
//         if (!string.IsNullOrEmpty(GoogleUID)) IsGuestAccount = false;
//     }

//     public void MapToApple(string appleUID)
//     {
//         AppleUID = appleUID ?? string.Empty;
//         if (!string.IsNullOrEmpty(AppleUID)) IsGuestAccount = false;
//     }

//     public void UpdateLastLoginTime()
//     {
//         LastLoginTime = DateTime.UtcNow;
//     }

//     public void SetUnityPlayerId(string unityPlayerId)
//     {
//         if (!string.IsNullOrEmpty(unityPlayerId) && PlayerId != unityPlayerId)
//         {
//             PlayerId = unityPlayerId;
//         }
//     }

//     public bool HasMappedAccount() => !string.IsNullOrEmpty(GoogleUID) || !string.IsNullOrEmpty(AppleUID);

//     // --- Private Helpers ---
//     private bool IsValidState()
//     {
//         if (string.IsNullOrEmpty(DeviceId))
//         {
//             Debug.LogError("[DeviceMappingData] DeviceId is null or empty.");
//             return false;
//         }
//         if (_firestore == null)
//         {
//             Debug.LogError("[DeviceMappingData] Firestore instance is null.");
//             return false;
//         }
//         return true;
//     }

//     private void SetDefaultValues()
//     {
//         string deviceIdShort = DeviceId?.Length > 6 ? DeviceId.Substring(0, 6) : "Guest";
//         GoogleUID = string.Empty;
//         AppleUID = string.Empty;
//         PlayerId = string.Empty;
//         Nickname = $"Player_{deviceIdShort}";
//         LastLoginTime = DateTime.UtcNow;
//         IsGuestAccount = true;
//     }

//     private void CopyFrom(DeviceMappingData source)
//     {
//         if (source == null) return;
//         // DeviceId와 IsLoaded는 복사하지 않음
//         GoogleUID = source.GoogleUID;
//         AppleUID = source.AppleUID;
//         PlayerId = source.PlayerId;
//         Nickname = source.Nickname;
//         LastLoginTime = source.LastLoginTime;
//         IsGuestAccount = source.IsGuestAccount;
//     }

//     private void LogTaskCompletion(Task task)
//     {
//         if (task.IsFaulted)
//         {
//             Debug.LogError($"[DeviceMappingData] Task failed: {task.Exception}");
//         }
//     }


// } 