// using Firebase;
// using Firebase.Extensions;
// using Firebase.RemoteConfig;
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Firebase.Firestore;
// using System.Threading.Tasks;
// using VContainer.Unity;
// using VContainer;

// public class FirebaseManager : InitBase
// {
//     private FirebaseApp m_App;
//     //Remote Config
//     private FirebaseRemoteConfig m_RemoteConfig;
//     private bool m_IsRemoteConfigInit = false;
//     private Dictionary<string, object> m_RemoteConfigDic = new Dictionary<string, object>();
//     //Firestore Database
//     private FirebaseFirestore m_FirebaseDatabase;
//     private bool m_IsFirestoreInit = false;

//     public FirebaseFirestore FirestoreInstance => m_FirebaseDatabase;

//     void Awake()
//     {
//         // Call Init directly from Awake
//         Init(); 
//         // Ensure DontDestroyOnLoad is active
//         DontDestroyOnLoad(gameObject); 
//     }

//     public override bool Init()
//     {
//         if (base.Init() == false)
//             return false;

//         StartCoroutine(InitFirebaseServiceCo()); // Start Firebase initialization
//         return true;
//     }
        

//     public bool IsInit()
//     {
//         return m_IsRemoteConfigInit 
//             && m_IsFirestoreInit;
//     }

//     private IEnumerator InitFirebaseServiceCo()
//     {
//         FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
//         {
//             var dependencyStatus = task.Result;
//             if(dependencyStatus == DependencyStatus.Available)
//             {
//                 Debug.Log($"FirebaseApp initialization success.");

//                 m_App = FirebaseApp.DefaultInstance;
//                 InitRemoteConfig();
//                 InitFirestore();
//             }
//             else
//             {
//                 Debug.LogError($"FirebaseApp initialization failed. DependencyStatus:{dependencyStatus}");
//             }
//         });

//         var elapsedTime = 0f;
//         while(elapsedTime < Define.THIRD_PARTY_SERVICE_INIT_TIME)
//         {
//             if(IsInit())
//             {
//                 Debug.Log($"{GetType()} initialization success.");
//                 yield break;
//             }

//             elapsedTime += Time.deltaTime;
//             yield return null;
//         }

//         Debug.LogError($"FirebaseApp initialization failed.");
//     }

    
//     #region REMOTE_CONFIG
//     private void InitRemoteConfig()
//     {
//         m_RemoteConfig = FirebaseRemoteConfig.DefaultInstance;
//         if(m_RemoteConfig == null)
//         {
//             Debug.LogError($"FirebaseApp initialization failed. FirebaseRemoteConfig is null.");
//             return;
//         }

//         m_RemoteConfigDic.Add("dev_app_version", string.Empty);
//         m_RemoteConfigDic.Add("real_app_version", string.Empty);

//         m_RemoteConfig.SetDefaultsAsync(m_RemoteConfigDic).ContinueWithOnMainThread(task =>
//         {
//             m_RemoteConfig.FetchAsync(TimeSpan.Zero).ContinueWithOnMainThread(fetchTask =>
//             {
//                 if(fetchTask.IsCompleted)
//                 {
//                     m_RemoteConfig.ActivateAsync().ContinueWithOnMainThread(activateTask =>
//                     {
//                         if(activateTask.IsCompleted)
//                         {
//                             //Get values from Remote Config
//                             m_RemoteConfigDic["dev_app_version"] = m_RemoteConfig.GetValue("dev_app_version").StringValue;
//                             m_RemoteConfigDic["real_app_version"] = m_RemoteConfig.GetValue("real_app_version").StringValue;
//                             m_IsRemoteConfigInit = true;
//                             Debug.Log($"FirebaseRemoteConfig initialization success.dev_app_version {m_RemoteConfigDic["dev_app_version"]}");
//                             Debug.Log($"FirebaseRemoteConfig initialization success.real_app_version {m_RemoteConfigDic["real_app_version"]}");
//                         }
//                     });
//                 }
//             });
//         });
//     }

//     public string GetAppVersion()
//     {
// #if DEV_VER
//         if(m_RemoteConfigDic.ContainsKey("dev_app_version"))
//         {
//             return m_RemoteConfigDic["dev_app_version"].ToString();
//         }
// #else
//         if(m_RemoteConfigDic.ContainsKey("real_app_version"))
//         {
//             return m_RemoteConfigDic["real_app_version"].ToString();
//         }
// #endif

//         return string.Empty;
//     }


//     #endregion


//     #region FIRESTORE
    
//     private void InitFirestore()
//     {
//         m_FirebaseDatabase = FirebaseFirestore.DefaultInstance;
//         if(m_FirebaseDatabase == null)
//         {
//             Debug.LogError($"FirebaseFirestore initialization failed. Instance is null.");
//             return;
//         }
//         m_IsFirestoreInit = true;
//         Debug.Log("FirebaseFirestore initialization success.");
//     }

//     protected void Dispose()
//     {
//         // base.Dispose();
//     }


//     #endregion
// }

