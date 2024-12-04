using UnityEngine;

public class BrushRollInteraction : MonoBehaviour
{
    public Material boneMaterial;           // 교체할 새로운 Material (Bone)
    public Material spineColorMaterial;     // 기존 Material (Spine_color)
    public GameObject cleanObject;         // Clean 오브젝트 (브러쉬 질 시 활성화/비활성화 할 오브젝트)

    private int shakeCount = 0;             // 털어내기 횟수 추적
    private bool isColliding = false;       // 충돌 상태 추적

    private void Start()
    {
        // Clean 오브젝트 초기 상태를 비활성화
        if (cleanObject != null)
        {
            cleanObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌이 시작되면 충돌 상태를 기록하고 Clean 오브젝트 활성화
        if (collision.gameObject.GetComponent<MeshRenderer>() != null)
        {
            isColliding = true;

            // Clean 오브젝트 활성화
            if (cleanObject != null)
            {
                cleanObject.SetActive(true);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // 충돌 상태일 때 계속해서 처리
        if (isColliding && collision.gameObject.GetComponent<MeshRenderer>() != null)
        {
            // 추가적인 조건을 넣을 수 있지만, 현재는 털어내는 행동으로 횟수만 추적
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // 충돌이 종료되었을 때 털어내기 횟수 증가
        if (collision.gameObject.GetComponent<MeshRenderer>() != null)
        {
            shakeCount++; // 털어내는 행위 횟수 증가

            // 털어내는 횟수가 15회 이상이면 Material 변경
            if (shakeCount >= 15)
            {
                MeshRenderer targetMeshRenderer = collision.gameObject.GetComponent<MeshRenderer>();
                if (targetMeshRenderer != null && targetMeshRenderer.sharedMaterial == spineColorMaterial)
                {
                    // Material을 Bone으로 교체 (새로운 인스턴스가 아닌 sharedMaterial을 수정)
                    targetMeshRenderer.sharedMaterial = boneMaterial;
                }

                // 털어내기 횟수 초기화
                shakeCount = 0;
            }

            // 'Clean' 오브젝트 활성화 및 비활성화
            if (cleanObject != null)
            {
                cleanObject.SetActive(false); // 활성화 상태를 반전시킴
            }

            // 충돌 상태 초기화
            isColliding = false;
        }
    }
}
