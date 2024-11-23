using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TMController : MonoBehaviour
{
    public GameObject menuCanvas; // UI Canvas
    public GameObject Anchor; // Controller Anchor (e.g., LeftHandControllerAnchor)
    public InputActionProperty toggleAction; // Input Action (Button Press)
    public float followSpeed = 5f; // ��ġ ��ȭ �ӵ�
    public float rotationSpeed = 5f; // ȸ�� ��ȭ �ӵ�
    public float maxDistance = 0.5f; // �ִ� �Ÿ� ����
    public Vector3 positionOffset = new Vector3(0, 0, 0.3f); // ��Ʈ�ѷ� �������� ������ �̵��� ������

    private bool isMenuVisible = false;

    private void OnEnable()
    {
        // Input Action Ȱ��ȭ
        toggleAction.action.Enable();
        toggleAction.action.performed += ToggleMenu;
    }

    private void OnDisable()
    {
        // Input Action ��Ȱ��ȭ
        toggleAction.action.performed -= ToggleMenu;
        toggleAction.action.Disable();
    }

    private void ToggleMenu(InputAction.CallbackContext context)
    {
        // �޴� Ȱ��ȭ/��Ȱ��ȭ ��ȯ
        isMenuVisible = !isMenuVisible;
        menuCanvas.SetActive(isMenuVisible);

        // �ʱ� ��ġ�� ȸ�� ����
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
        // Anchor�� ��ġ�� ȸ���� ������� �޴��� ��ġ �� ȸ�� ����
        Vector3 targetPosition = Anchor.transform.position + Anchor.transform.forward * positionOffset.z
                                + Anchor.transform.right * positionOffset.x
                                + Anchor.transform.up * positionOffset.y;

        // �Ÿ� ���� ����
        if (Vector3.Distance(menuCanvas.transform.position, targetPosition) > maxDistance)
        {
            targetPosition = menuCanvas.transform.position +
                             (targetPosition - menuCanvas.transform.position).normalized * maxDistance;
        }

        // ��ġ�� ȸ�� ������Ʈ (�ε巴�� �̵�)
        menuCanvas.transform.position = Vector3.Lerp(
            menuCanvas.transform.position,
            targetPosition,
            Time.deltaTime * followSpeed
        );

        // ȸ���� �ε巴�� ���󰡱�
        Quaternion targetRotation = Anchor.transform.rotation;
        menuCanvas.transform.rotation = Quaternion.Lerp(
            menuCanvas.transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }
}
