using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InventorySlot : MonoBehaviour
{
    public bool isOccupied = false; // ������ ��� �ִ��� ����
    private GameObject storedItem; // ���Կ� ����� ������

    void OnTriggerEnter(Collider other)
    {
        if (!isOccupied && other.CompareTag("Grabbable"))
        {
            // ���Կ� ������ ����
            storedItem = other.gameObject;
            isOccupied = true;

            // �������� ���� ��ġ�� ����
            storedItem.transform.position = transform.position;
            storedItem.transform.rotation = transform.rotation;

            // XR Grab Interactable ��Ȱ��ȭ
            storedItem.GetComponent<XRGrabInteractable>().enabled = false;

            Debug.Log($"{storedItem.name} has been stored in the slot.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (isOccupied && other.gameObject == storedItem)
        {
            // ���Կ��� ������ ����
            storedItem.GetComponent<XRGrabInteractable>().enabled = true;
            isOccupied = false;
            storedItem = null;

            Debug.Log("Item has been removed from the slot.");
        }
    }
}