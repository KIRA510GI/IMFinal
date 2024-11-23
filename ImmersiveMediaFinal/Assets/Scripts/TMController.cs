using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TMController : MonoBehaviour
{
    public GameObject menuCanvas; // UI Canvas
    public GameObject Anchor; // Controller Anchor (e.g., LeftHandControllerAnchor)
    public InputActionProperty toggleAction; // Input Action (Button Press)
    public float followSpeed = 5f; // 위치 변화 속도
    public float rotationSpeed = 5f; // 회전 변화 속도
    public float maxDistance = 0.5f; // 최대 거리 제한
    public Vector3 positionOffset = new Vector3(0, 0, 0.3f); // 컨트롤러 기준으로 앞으로 이동할 오프셋

    private bool isMenuVisible = false;

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
        menuCanvas.SetActive(isMenuVisible);

        // 초기 위치와 회전 설정
        if (isMenuVisible)
        {
            UpdateMenuPositionAndRotation();
        }
    }

    private void Update()
    {
        if (isMenuVisible)
        {
            UpdateMenuPositionAndRotation();
        }
    }

    private void UpdateMenuPositionAndRotation()
    {
        // Anchor의 위치와 회전을 기반으로 메뉴의 위치 및 회전 설정
        Vector3 targetPosition = Anchor.transform.position + Anchor.transform.forward * positionOffset.z
                                + Anchor.transform.right * positionOffset.x
                                + Anchor.transform.up * positionOffset.y;

        // 거리 제한 적용
        if (Vector3.Distance(menuCanvas.transform.position, targetPosition) > maxDistance)
        {
            targetPosition = menuCanvas.transform.position +
                             (targetPosition - menuCanvas.transform.position).normalized * maxDistance;
        }

        // 위치와 회전 업데이트 (부드럽게 이동)
        menuCanvas.transform.position = Vector3.Lerp(
            menuCanvas.transform.position,
            targetPosition,
            Time.deltaTime * followSpeed
        );

        // 회전도 부드럽게 따라가기
        Quaternion targetRotation = Anchor.transform.rotation;
        menuCanvas.transform.rotation = Quaternion.Lerp(
            menuCanvas.transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }
}
