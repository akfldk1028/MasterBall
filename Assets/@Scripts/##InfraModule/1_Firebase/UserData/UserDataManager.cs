// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using System.Threading.Tasks;
// using Firebase.Firestore;
// using VContainer;

// public class UserDataManager
// {
//     private readonly FirebaseManager _firebaseManager;

//     public string CurrentPlayerId { get; private set; }

//     public bool ExistsSavedData { get; private set; }
//     public List<IUserData> UserDataList { get; private set; } = new List<IUserData>();

//     [Inject]
//     public UserDataManager(FirebaseManager firebaseManager)
//     {
//         _firebaseManager = firebaseManager ?? throw new System.ArgumentNullException(nameof(firebaseManager));
//         Debug.Log("[UserDataManager] FirebaseManager injected.");
//     }

//     public bool Init()
//     {
//         FirebaseFirestore firestore = _firebaseManager.FirestoreInstance;
//         if (firestore == null)
//         {
//             Debug.LogError("[UserDataManager] Cannot initialize: FirebaseFirestore instance is null.");
//             return false;
//         }

//         UserDataList.Add(new DeviceMappingData(firestore));
//         UserDataList.Add(new UserGoodsData(firestore));
//         UserDataList.Add(new UserFullGameProgress(firestore));
//         UserDataList.Add(new UserProfile(firestore));
//         Debug.Log($"[UserDataManager] UserData initialized. Total types: {UserDataList.Count}");

//         return true;
//     }

//     public void SetDefaultUserData()
//     {
//         for (int i = 0; i < UserDataList.Count; i++)
//         {
//             UserDataList[i].SetDefaultData();
//         }
//     }

//     public void LoadUserData()
//     {
//         for (int i = 0; i < UserDataList.Count; i++)
//         {
//             UserDataList[i].LoadData();
//         }
//     }

//     public void SaveUserData()
//     {
//         for (int i = 0; i < UserDataList.Count; i++)
//         {
//             UserDataList[i].SaveData();
//         }
//     }

//     public T GetUserData<T>() where T : class, IUserData
//     {
//         return UserDataList.OfType<T>().FirstOrDefault();
//     }

//     /// <summary>
//     /// 현재 사용자 PlayerId를 설정하고, 필요한 데이터 객체에 컨텍스트를 전달합니다.
//     /// </summary>
//     public void SetCurrentPlayerId(string playerId)
//     {
//         if (string.IsNullOrEmpty(playerId))
//         {
//             Debug.LogError("[UserDataManager] Attempted to set an invalid PlayerId.");
//             return;
//         }
        
//         CurrentPlayerId = playerId;
//         Debug.Log($"[UserDataManager] Current PlayerId set to: {playerId}");

//         // PlayerId 컨텍스트가 필요한 데이터 객체에 PlayerId 전달
//         foreach (var userData in UserDataList)
//         {
//             if (userData is UserGoodsData goodsData)
//             {
//                 goodsData.SetPlayerContext(playerId);
//             }
//             else if (userData is UserFullGameProgress progressData)
//             {
//                 progressData.SetPlayerContext(playerId);
//             }
//             else if (userData is UserProfile profileData)
//             {
//                 profileData.SetPlayerContext(playerId);
//             }
//             // 다른 IUserData 타입도 PlayerId가 필요하다면 여기에 추가
//             // else if (userData is UserQuestData questData)
//             // {
//             //     questData.SetPlayerContext(playerId);
//             // }
//         }

//         // PlayerId 설정 후 데이터 로드를 트리거할 수 있음
//         // LoadUserData(); 
//     }

//     /// <summary>
//     /// 특정 사용자 데이터 하나만 비동기적으로 저장합니다.
//     /// </summary>
//     /// <param name="userData">저장할 IUserData 객체</param>
//     public async Task SaveSpecificUserDataAsync(IUserData userData)
//     {
//         if (userData == null)
//         {
//             Debug.LogError("Cannot save null user data.");
//             return;
//         }

//         if (!UserDataList.Contains(userData))
//         {
//             Debug.LogWarning("Saving user data that is not managed by UserDataManager. Ensure this is intended.");
//         }

//         try
//         {
//             // 각 데이터 객체가 자신의 SaveDataAsync 로직을 수행하도록 호출
//             bool success = await userData.SaveDataAsync();
//             if (success)
//             {
//                 Debug.Log($"[{nameof(UserDataManager)}] Successfully requested save for {userData.GetType().Name}.");
//             }
//             else
//             {
//                  Debug.LogError($"[{nameof(UserDataManager)}] Save request failed for {userData.GetType().Name}.");
//             }

//         }
//         catch (System.Exception e)
//         {
//             Debug.LogError($"[{nameof(UserDataManager)}] Failed to save specific user data ({userData.GetType().Name}): {e.Message}");
//         }
//     }
// }
