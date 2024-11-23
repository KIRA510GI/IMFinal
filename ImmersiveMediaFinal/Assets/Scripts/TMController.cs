using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TMController : MonoBehaviour
{
    public GameObject menuCanvas; // UI Canvas
    public GameObject Anchor; // Controller Anchor (e.g., LeftHandControllerAnchor)

    public InputActionProperty toggleAction; // Input Action (Button Press)

    public float followSpeed; // ��ġ ��ȭ �ӵ� (�ﰢ������ ��������� ����)
    public float rotationSpeed; // ȸ�� ��ȭ �ӵ�
    public float maxDistance; // �ִ� �Ÿ� ���� (�ſ� ������)

    public Vector3 positionOffset = new Vector3(); // ��Ʈ�ѷ��� �� ����� ������

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
            UpdateMenuPositionAndRotation(true); // ��� ��ġ �� ȸ�� ����
        }
    }

    private void Update()
    {
        if (isMenuVisible)
        {
            UpdateMenuPositionAndRotation(false); // �ε巴�� ��ġ �� ȸ�� �̵�
        }
    }

    private void UpdateMenuPositionAndRotation(bool instant)
    {
        // Anchor�� ��ġ�� ȸ���� ������� �޴��� ��ġ ����
        Vector3 targetPosition = Anchor.transform.position
                                + Anchor.transform.forward * positionOffset.z
                                + Anchor.transform.right * positionOffset.x
                                + Anchor.transform.up * positionOffset.y;

        // �Ÿ� ����: ��Ʈ�ѷ��� �ſ� ������ ����
        if (Vector3.Distance(menuCanvas.transform.position, targetPosition) > maxDistance)
        {
            targetPosition = Anchor.transform.position +
                             (targetPosition - Anchor.transform.position).normalized * maxDistance;
        }

        // ��ġ�� ȸ�� ������Ʈ (��� �̵� �Ǵ� �ε巴�� �̵�)
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
