using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventorySlot : MonoBehaviour
{
    public GameObject ItemInSlot;  // 슬롯에 저장된 아이템
    [SerializeField] private Image slotImage;  // 슬롯 이미지
    private Color originalColor;  // 원래 색상 저장

    public GameObject attachPoint;  // AttachPoint 위치
    public InputActionProperty grabAction;  // 잡기 액션

    void Start()
    {
        if (slotImage == null)
        {
            slotImage = GetComponent<Image>();
            if (slotImage == null)
            {
                Debug.LogError($"[InventorySlot] {name}에 Image 컴포넌트가 없습니다!");
                return;
            }
        }

        originalColor = slotImage.color;
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject obj = other.gameObject;

        // 슬롯에 이미 아이템이 있는 경우
        if (ItemInSlot != null)
        {
            if (grabAction.action.ReadValue<float>() > 0.1f)
            {
                RemoveItem(ItemInSlot);
            }
            return;
        }

        // 아이템인지 확인하고, 잡기가 끝났을 때 슬롯에 추가
        if (IsItem(obj) && grabAction.action.ReadValue<float>() < 0.1f)
        {
            InsertItem(obj);
        }
    }

    bool IsItem(GameObject obj)
    {
        return obj.GetComponent<Item>() != null;
    }

    // 아이템 삽입
    public void InsertItem(GameObject obj)
    {
        ItemInSlot = obj;
        obj.transform.position = attachPoint.transform.position;
        obj.transform.rotation = attachPoint.transform.rotation;
        obj.SetActive(true);  // 아이템 활성화

        slotImage.color = Color.green;

        Item itemScript = obj.GetComponent<Item>();
        if (itemScript != null)
        {
            itemScript.inSlot = true;
            itemScript.CurrentSlot = this;
        }
    }

    // 아이템 제거
    public void RemoveItem(GameObject obj)
    {
        if (ItemInSlot != null)
        {
            Item itemScript = obj.GetComponent<Item>();
            if (itemScript != null)
            {
                itemScript.inSlot = false;
                itemScript.CurrentSlot = null;
            }

            obj.SetActive(false);  // 아이템 비활성화
            ItemInSlot = null;
            slotImage.color = originalColor;
        }
    }
}
