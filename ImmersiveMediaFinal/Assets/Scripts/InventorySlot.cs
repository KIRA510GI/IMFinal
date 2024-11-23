using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InventorySlot : MonoBehaviour
{
    public bool isOccupied = false; // 슬롯이 비어 있는지 여부
    private GameObject storedItem; // 슬롯에 저장된 아이템

    void OnTriggerEnter(Collider other)
    {
        if (!isOccupied && other.CompareTag("Grabbable"))
        {
            // 슬롯에 아이템 보관
            storedItem = other.gameObject;
            isOccupied = true;

            // 아이템을 슬롯 위치로 고정
            storedItem.transform.position = transform.position;
            storedItem.transform.rotation = transform.rotation;

            // XR Grab Interactable 비활성화
            storedItem.GetComponent<XRGrabInteractable>().enabled = false;

            Debug.Log($"{storedItem.name} has been stored in the slot.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (isOccupied && other.gameObject == storedItem)
        {
            // 슬롯에서 아이템 제거
            storedItem.GetComponent<XRGrabInteractable>().enabled = true;
            isOccupied = false;
            storedItem = null;

            Debug.Log("Item has been removed from the slot.");
        }
    }
}