using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventorySlot : MonoBehaviour
{
    public GameObject ItemInSlot; // ���Կ� ����� ������
    public Image slotImage; // ���� �̹��� UI
    private Color originalColor; // ������ ���� ����

    public InputActionProperty grabAction; // ��� �׼��� ���� InputActionProperty

    void Start()
    {
        slotImage = GetComponent<Image>();
        originalColor = slotImage.color;
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject obj = other.gameObject;

        // ���Կ� �̹� �������� �ִ� ���
        if (ItemInSlot != null)
        {
            // ����ڰ� ��� ������ �ϸ� ���Կ��� �������� ����
            if (grabAction.action.ReadValue<float>() > 0.1f)
            {
                RemoveItem(ItemInSlot);
            }
            return;
        }

        // ���ο� �������� �߰��Ϸ��� ���������� Ȯ��
        if (!IsItem(obj)) return;

        // ����ڰ� ��⸦ ������ �� �������� ���Կ� ����
        if (grabAction.action.ReadValue<float>() < 0.1f)
        {
            InsertItem(obj);
            if (grabAction.action.ReadValue<float>() > 0.1f)
            {
                RemoveItem(ItemInSlot);
            }
        }
    }

    // ���������� Ȯ��
    bool IsItem(GameObject obj)
    {
        return obj.GetComponent<Item>() != null;
    }

    // ���Կ� ������ �߰�
    void InsertItem(GameObject obj)
    {
        Rigidbody objRigidbody = obj.GetComponent<Rigidbody>();
        if (objRigidbody != null)
        {
            objRigidbody.isKinematic = true; // ���� ���� ��Ȱ��ȭ
        }

        // ������ �ڽ����� ����
        obj.transform.SetParent(transform, true);

        // ������ ��ġ �� ȸ�� ����
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = obj.GetComponent<Item>().slotRotation;

        // ������ ���� ������Ʈ
        Item itemScript = obj.GetComponent<Item>();
        itemScript.inSlot = true;
        itemScript.CurrentSlot = this;

        // ���� ���� ������Ʈ
        ItemInSlot = obj;
        slotImage.color = Color.blue; // ���� ���� ����
    }

    // ���Կ��� ������ ����
    public void RemoveItem(GameObject obj)
    {
        // ������ ���� Ȯ��
        Item itemScript = obj.GetComponent<Item>();
        if (itemScript == null || !itemScript.inSlot) return;

        // ���� ���� �ʱ�ȭ
        ItemInSlot = null;

        // �θ� ���� ����
        obj.transform.parent = null;

        // ���� ���� Ȱ��ȭ
        Rigidbody objRigidbody = obj.GetComponent<Rigidbody>();
        if (objRigidbody != null)
        {
            objRigidbody.isKinematic = false;
        }

        // ������ ���� �ʱ�ȭ
        itemScript.inSlot = false;
        itemScript.CurrentSlot = null;

        // ���� ���� �ʱ�ȭ
        ResetColor();
    }

    // ���� ���� �ʱ�ȭ
    public void ResetColor()
    {
        slotImage.color = originalColor;
    }
}
