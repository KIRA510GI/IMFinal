using UnityEngine;

public class BrushRollInteraction : MonoBehaviour
{
    public Material boneMaterial;           // 교체할 새로운 Material (Bone)
    public Material spineColorMaterial;     // 기존 Material (Spine_color)
    public GameObject cleanObject;          // Clean 오브젝트 (브러쉬 질 시 활성화/비활성화 할 오브젝트)
    public GameObject[] allowedObjects;     // 충돌 반응을 허용할 오브젝트 배열
    public float movementThreshold = 0.1f; // 움직임 감지 기준 거리

    private int shakeCount = 0;             // 털어내기 횟수 추적
    private bool isColliding = false;       // 충돌 상태 추적
    private Vector3 lastPosition;           // 이전 프레임의 브러쉬 위치
    private float accumulatedMovement = 0; // 축적된 움직임 거리

    private void Start()
    {
        // Clean 오브젝트 초기 상태를 비활성화
        if (cleanObject != null)
        {
            cleanObject.SetActive(false);
        }

        Debug.Log("BrushRollInteraction 초기화 완료");
    }

    private bool IsAllowedObject(GameObject obj)
    {
        // 허용된 오브젝트 배열에 포함되어 있는지 확인
        foreach (GameObject allowedObject in allowedObjects)
        {
            if (allowedObject == obj)
            {
                return true;
            }
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsAllowedObject(collision.gameObject))
        {
            Debug.Log($"충돌 무시: {collision.gameObject.name}");
            return; // 허용되지 않은 오브젝트는 무시
        }

        if (collision.gameObject.GetComponent<MeshRenderer>() != null)
        {
            isColliding = true;
            lastPosition = transform.position;

            // Clean 오브젝트 활성화
            if (cleanObject != null)
            {
                cleanObject.SetActive(true);
            }

            Debug.Log($"충돌 시작: {collision.gameObject.name}");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!IsAllowedObject(collision.gameObject))
        {
            return; // 허용되지 않은 오브젝트는 무시
        }

        if (isColliding && collision.gameObject.GetComponent<MeshRenderer>() != null)
        {
            float movement = Vector3.Distance(transform.position, lastPosition);
            Debug.Log($"현재 움직임 거리: {movement}, 축적된 움직임: {accumulatedMovement}");

            if (movement > movementThreshold)
            {
                accumulatedMovement += movement;

                if (accumulatedMovement >= movementThreshold * 3)
                {
                    shakeCount++;
                    accumulatedMovement = 0;

                    Debug.Log($"털어내기 카운트 증가: {shakeCount}");

                    if (shakeCount >= 5)
                    {
                        MeshRenderer targetMeshRenderer = collision.gameObject.GetComponent<MeshRenderer>();
                        if (targetMeshRenderer != null && targetMeshRenderer.sharedMaterial == spineColorMaterial)
                        {
                            targetMeshRenderer.sharedMaterial = boneMaterial;
                            Debug.Log("Material이 Bone으로 변경되었습니다");
                        }

                        shakeCount = 0;
                    }
                }
            }

            lastPosition = transform.position; // 현재 위치를 갱신
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!IsAllowedObject(collision.gameObject))
        {
            return; // 허용되지 않은 오브젝트는 무시
        }

        if (collision.gameObject.GetComponent<MeshRenderer>() != null)
        {
            isColliding = false;
            accumulatedMovement = 0;

            // Clean 오브젝트 비활성화
            if (cleanObject != null)
            {
                cleanObject.SetActive(false);
            }

            Debug.Log($"충돌 종료: {collision.gameObject.name}");
        }
    }
}
