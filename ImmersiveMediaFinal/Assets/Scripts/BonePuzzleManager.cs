using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BonePuzzleManager : MonoBehaviour
{
    [System.Serializable]
    public class Bone
    {
        public XRGrabInteractable boneObject; // XR 조작 가능한 뼈
        public Transform correctPosition; // 올바른 위치 (Transform)
        public Material correctMaterial; // 올바른 Material (Bone)
    }

    public Bone[] bones;
    private bool _allBonesCorrect = false; // 모든 뼈가 맞춰졌는지 확인
    private bool _messageDisplayed = false; // 메세지 출력 여부 관리
    
    public string targetObjectName = "Tyrannosaurus_SKEL5-full";
    public GameObject animatedTyrannosaurus; // T-Rex(animated) 오브젝트

    private void Update()
    {
        // 모든 뼈가 맞춰졌는지 확인
        CheckBonePosition();
    }

    private void CheckBonePosition()
    {
        if (_messageDisplayed) return;

        _allBonesCorrect = true;
        
        foreach (var bone in bones)
        {
            // 뼈가 정확히 정답 위치에 있는지 확인하고, Material이 올바른지 확인
            if (!IsBoneAtCorrectPosition(bone))
            {
                _allBonesCorrect = false;
                break;
            }
        }

        // 모든 뼈가 올바른 위치에 있고, 올바른 Material을 가진 경우
        if (_allBonesCorrect && !_messageDisplayed)
        {
            Debug.Log("All Bones Correct");
            HideTargetObjectAndBones(); // 뼈를 숨기는 메서드 호출
            ShowAnimatedTyrannosaurus();
            _messageDisplayed = true; // 메시지 출력 후 상태를 업데이트
        }
    }

    private bool IsBoneAtCorrectPosition(Bone bone)
    {
        // 뼈의 위치와 회전이 정확하고, Material이 올바른지 확인
        bool isAtCorrectPosition = bone.boneObject.transform.position == bone.correctPosition.position &&
                                    bone.boneObject.transform.rotation == bone.correctPosition.rotation;

        bool hasCorrectMaterial = bone.boneObject.GetComponent<Renderer>().sharedMaterial == bone.correctMaterial;

        return isAtCorrectPosition && hasCorrectMaterial;
    }

    private void HideTargetObjectAndBones()
    {
        // 지정된 이름의 오브젝트르 찾음
        GameObject targetObject = GameObject.Find(targetObjectName);
        
        // 대상 오브젝트 비활성화
        if (targetObject != null)
        {
            targetObject.SetActive(false);
            Debug.Log($"Target object '{targetObjectName}' has been hidden.");
        }
        else
        {
            Debug.LogWarning($"Target object '{targetObjectName}' was not found.");
        }
        
        // 배열에 있는 모든 뼈를 비활성화
        foreach (var bone in bones)
        {
            if (bone.boneObject != null)
            {
                bone.boneObject.gameObject.SetActive(false);
                Debug.Log($"Bone Object '{bone.boneObject.name}' has been hidden.");
            }
        }
    }

    private void ShowAnimatedTyrannosaurus()
    {
        // 애니메이션 오브젝트를 활성화
        if (animatedTyrannosaurus != null)
        {
            animatedTyrannosaurus.SetActive(true);
            Debug.Log($"Animated T-Rex has been shown.");
        }
        else
        {
            Debug.LogWarning($"Animated T-Rex object is not assigned in the inspector.");
        }
    }
}
