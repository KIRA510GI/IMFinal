using UnityEngine;
using UnityEngine.InputSystem;

public class TMController : MonoBehaviour
{
    public GameObject menuCanvas; // UI Canvas
    public GameObject Anchor; // Controller Anchor (e.g., LeftHandControllerAnchor)

    public InputActionProperty toggleAction; // Input Action (Button Press)

    public float followSpeed; // 위치 변화 속도
    public float rotationSpeed; // 회전 변화 속도
    public float maxDistance; // 최대 거리 제한

    public Vector3 positionOffset = new Vector3(); // 컨트롤러와 더 가까운 오프셋

    private bool isMenuVisible = false;

    // 4개의 슬롯 배열
    public InventorySlot[] inventorySlots;

    private void OnEnable()
    {
        // Input Action 활성화
        toggleAction.action.Enable();
        toggleAction.action.performed += ToggleMenu;
    }

    private void OnDisable()
    {
        // Input Action 비활성화
        toggleAction.action.performed -= ToggleMenu;
        toggleAction.action.Disable();
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        // 메뉴 활성화/비활성화 전환
        isMenuVisible = !isMenuVisible;
    }

    private void Update()
    {
        UpdateMenuPositionAndRotation(false); // 메뉴의 위치와 회전을 항상 업데이트
    }

    private void UpdateMenuPositionAndRotation(bool instant)
    {
        // 메뉴의 목표 위치를 계산
        Vector3 targetPosition = Anchor.transform.position
                                 + Anchor.transform.forward * positionOffset.z
                                 + Anchor.transform.right * positionOffset.x
                                 + Anchor.transform.up * positionOffset.y;

        if (isMenuVisible)
        {
            // 메뉴가 활성화된 상태일 때, 플레이어에게 가까운 위치로 설정
            float distanceToTarget = Vector3.Distance(menuCanvas.transform.position, targetPosition);
        
            // 메뉴가 활성화되었을 때 더 빠르게 이동하도록 속도 증가
            if (distanceToTarget > 0.5f)  // 목표 위치와의 거리가 일정 이상일 때만 빠르게 이동
            {
                followSpeed = 20f;  // 이동 속도를 크게 설정 (기존보다 더 빠르게)
                rotationSpeed = 15f;  // 회전 속도도 빠르게 설정
            }
            else
            {
                // 목표에 거의 도달한 경우, 속도를 좀 더 낮춰 부드럽게 마무리
                followSpeed = 10f;  // 목표에 도달할 때는 속도를 줄여서 부드럽게
                rotationSpeed = 10f; 
            }

            // 메뉴가 너무 멀어지지 않도록 maxDistance로 제한
            if (distanceToTarget > maxDistance)
            {
                targetPosition = Anchor.transform.position + (targetPosition - Anchor.transform.position).normalized * maxDistance;
            }
        }
        else
        {
            // 메뉴가 비활성화된 상태일 때, 메뉴를 멀리 이동시켜서 보이지 않게 처리
            targetPosition = Anchor.transform.position + (Anchor.transform.forward * 30f); // 원하는 만큼 멀리 이동
        }

        // 메뉴가 활성화되었을 때의 부드러운 이동
        if (instant)
        {
            menuCanvas.transform.position = targetPosition;
            menuCanvas.transform.rotation = Anchor.transform.rotation;
        }
        else
        {
            // 더 빠르게 이동하도록 Lerp 속도 증가
            menuCanvas.transform.position = Vector3.Lerp(menuCanvas.transform.position, targetPosition, Time.deltaTime * followSpeed);
            Quaternion targetRotation = Anchor.transform.rotation;
            menuCanvas.transform.rotation = Quaternion.Lerp(menuCanvas.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
