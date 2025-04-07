using UnityEngine;
using UnityEngine.UI; // InputField 사용을 위해 추가
using VContainer; // UserDataManager 주입을 위해 (프로젝트 설정에 따라 다를 수 있음)
using System.Threading.Tasks; // 비동기 작업을 위해

public class ProfileUIScript : MonoBehaviour
{
    // UserDataManager 주입 (VContainer 사용 예시)
    private UserDataManager _userDataManager;

    [SerializeField] private InputField nicknameInputField;
    [SerializeField] private Button saveButton;
    [SerializeField] private Text currentNicknameText; // 현재 닉네임 표시용 (선택 사항)

    [Inject]
    public void Construct(UserDataManager userDataManager)
    {
        _userDataManager = userDataManager;
        Debug.Log("ProfileUIScript: UserDataManager injected.");
    }

    void Start()
    {
        // 버튼 클릭 리스너 설정
        saveButton.onClick.AddListener(OnSaveNicknameClicked);

        // 초기 닉네임 로드 및 표시 (데이터 로딩이 완료된 후 호출되어야 함)
        // 실제로는 UserDataManager 로딩 완료 후 이 UI를 활성화하거나 데이터를 갱신하는 로직 필요
        LoadAndDisplayCurrentNickname();
    }

    // 현재 닉네임 로드 및 표시 (예시)
    void LoadAndDisplayCurrentNickname()
    {
        UserProfile profile = _userDataManager.GetUserData<UserProfile>();
        if (profile != null && profile.IsLoaded)
        {
            nicknameInputField.text = profile.Nickname;
            if(currentNicknameText != null) currentNicknameText.text = profile.Nickname;
        }
        else
        {
             Debug.LogWarning("Profile data not loaded yet. Cannot display nickname.");
             // 필요하다면 로딩 중 표시 또는 재시도 로직 추가
        }
    }


    // "닉네임 저장" 버튼 클릭 시 호출될 메서드
    private async void OnSaveNicknameClicked()
    {
        string newNickname = nicknameInputField.text;

        // 입력값 유효성 검사 (예: 빈 값, 길이 제한 등)
        if (string.IsNullOrWhiteSpace(newNickname))
        {
            Debug.LogWarning("Nickname cannot be empty.");
            // 사용자에게 피드백 (예: 오류 메시지 표시)
            return;
        }

        // UserProfile 데이터 가져오기
        UserProfile profile = _userDataManager.GetUserData<UserProfile>();

        // 데이터가 로드되었는지 확인
        if (profile == null || !profile.IsLoaded)
        {
            Debug.LogError("UserProfile data is not loaded yet. Cannot save nickname.");
            // 사용자에게 피드백
            return;
        }

        // --- 상태 변경 및 저장 ---
        try
        {
            // 1. 상태 변경 (메모리 내 데이터 업데이트)
            profile.UpdateNickname(newNickname);
            Debug.Log($"Nickname updated in memory to: {profile.Nickname}");

             if(currentNicknameText != null) currentNicknameText.text = profile.Nickname; // UI 즉시 업데이트

            // 2. Firestore에 저장 (비동기 호출)
            Debug.Log("Requesting save to Firestore...");
            bool saveSuccess = await profile.SaveDataAsync(); // UserProfile의 SaveDataAsync 호출

            // --- 저장 결과 처리 ---
            if (saveSuccess)
            {
                Debug.Log("Nickname successfully saved to Firestore.");
                // 사용자에게 성공 피드백 (예: "저장 완료" 메시지)
            }
            else
            {
                Debug.LogError("Failed to save nickname to Firestore.");
                // 사용자에게 실패 피드백
                // 이전 값으로 롤백 고려 (예: profile.Nickname = 이전값;) - 필요 시
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"An error occurred while saving nickname: {e.Message}");
            // 사용자에게 오류 피드백
        }
    }
}