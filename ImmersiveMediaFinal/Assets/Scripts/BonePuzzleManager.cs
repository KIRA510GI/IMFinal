using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BonePuzzleManager : MonoBehaviour
{
    [System.Serializable]
    public class Bone
    {
        public XRGrabInteractable boneObject; // XR 조작 가능한 뼈
        public Transform correctPosition; // 올바른 위치 (Transform)
    }

    public Bone[] bones;
    private bool allBonesCorrect = false; // 모든 뼈가 맞춰졌는지 확인
    private bool messageDisplayed = false; // 메세지 출력 여부 관리

    private void Update()
    {
        // 모든 뼈가 맞춰졌는지 확인
        CheckBonePosition();
    }

    private void CheckBonePosition()
    {
        if (messageDisplayed) return;

        allBonesCorrect = true;
        
        foreach (var bone in bones)
        {
            // 뼈가 정확히 정답 위치에 있는지 확인
            if (!IsBoneAtCorrectPosition(bone))
            {
                allBonesCorrect = false;
                break;
            }
        }

        // 모든 뼈가 올바른 위치에 있는 경우
        if (allBonesCorrect && !messageDisplayed)
        {
            Debug.Log("All Bones Correct");
            messageDisplayed = true; // 메시지 출력 후 상태를 업데이트
        }

    }

    private bool IsBoneAtCorrectPosition(Bone bone)
    {
        // 자동 스냅된 경우 정확한 위치에 있는 것으로 간주
        return bone.boneObject.transform.position == bone.correctPosition.position &&
            bone.boneObject.transform.rotation == bone.correctPosition.rotation;
    }
}
