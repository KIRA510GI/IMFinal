using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TMController : MonoBehaviour
{
    public GameObject menuCanvas; // UI Canvas
    public GameObject Anchor; // Controller Anchor (e.g., LeftHandControllerAnchor)

    public InputActionProperty toggleAction; // Input Action (Button Press)

    public float followSpeed; // 위치 변화 속도 (즉각적으로 따라오도록 증가)
    public float rotationSpeed; // 회전 변화 속도
    public float maxDistance; // 최대 거리 제한 (매우 가깝게)

    public Vector3 positionOffset = new Vector3(); // 컨트롤러와 더 가까운 오프셋

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
            UpdateMenuPositionAndRotation(true); // 즉시 위치 및 회전 설정
        }
    }

    private void Update()
    {
        if (isMenuVisible)
        {
            UpdateMenuPositionAndRotation(false); // 부드럽게 위치 및 회전 이동
        }
    }

    private void UpdateMenuPositionAndRotation(bool instant)
    {
        // Anchor의 위치와 회전을 기반으로 메뉴의 위치 설정
        Vector3 targetPosition = Anchor.transform.position
                                + Anchor.transform.forward * positionOffset.z
                                + Anchor.transform.right * positionOffset.x
                                + Anchor.transform.up * positionOffset.y;

        // 거리 제한: 컨트롤러에 매우 가깝게 유지
        if (Vector3.Distance(menuCanvas.transform.position, targetPosition) > maxDistance)
        {
            targetPosition = Anchor.transform.position +
                             (targetPosition - Anchor.transform.position).normalized * maxDistance;
        }

        // 위치와 회전 업데이트 (즉시 이동 또는 부드럽게 이동)
        if (instant)
        {
            menuCanvas.transform.position = targetPosition;
            menuCanvas.transform.rotation = Anchor.transform.rotation;
        }
        else
        {
            menuCanvas.transform.position = Vector3.Lerp(
                menuCanvas.transform.position,
                targetPosition,
                Time.deltaTime * followSpeed
            );

            Quaternion targetRotation = Anchor.transform.rotation;
            menuCanvas.transform.rotation = Quaternion.Lerp(
                menuCanvas.transform.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }
}
