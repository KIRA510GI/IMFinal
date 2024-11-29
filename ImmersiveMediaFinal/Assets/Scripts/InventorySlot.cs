using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventorySlot : MonoBehaviour
{
    public GameObject ItemInSlot; // 슬롯에 저장된 아이템
    public Image slotImage; // 슬롯 이미지 UI
    private Color originalColor; // 원래의 색상 저장

    public InputActionProperty grabAction; // 잡기 액션을 위한 InputActionProperty

    void Start()
    {
        slotImage = GetComponent<Image>();
        originalColor = slotImage.color;
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject obj = other.gameObject;

        // 슬롯에 이미 아이템이 있는 경우
        if (ItemInSlot != null)
        {
            // 사용자가 잡기 동작을 하면 슬롯에서 아이템을 제거
            if (grabAction.action.ReadValue<float>() > 0.1f)
            {
                RemoveItem(ItemInSlot);
            }
            return;
        }

        // 새로운 아이템을 추가하려면 아이템인지 확인
        if (!IsItem(obj)) return;

        // 사용자가 잡기를 멈췄을 때 아이템을 슬롯에 삽입
        if (grabAction.action.ReadValue<float>() < 0.1f)
        {
            InsertItem(obj);
            if (grabAction.action.ReadValue<float>() > 0.1f)
            {
                RemoveItem(ItemInSlot);
            }
        }
    }

    // 아이템인지 확인
    bool IsItem(GameObject obj)
    {
        return obj.GetComponent<Item>() != null;
    }

    // 슬롯에 아이템 추가
    void InsertItem(GameObject obj)
    {
        Rigidbody objRigidbody = obj.GetComponent<Rigidbody>();
        if (objRigidbody != null)
        {
            objRigidbody.isKinematic = true; // 물리 엔진 비활성화
        }

        // 슬롯의 자식으로 설정
        obj.transform.SetParent(transform, true);

        // 아이템 위치 및 회전 설정
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = obj.GetComponent<Item>().slotRotation;

        // 아이템 상태 업데이트
        Item itemScript = obj.GetComponent<Item>();
        itemScript.inSlot = true;
        itemScript.CurrentSlot = this;

        // 슬롯 정보 업데이트
        ItemInSlot = obj;
        slotImage.color = Color.blue; // 슬롯 색상 변경
    }

    // 슬롯에서 아이템 제거
    public void RemoveItem(GameObject obj)
    {
        // 아이템 상태 확인
        Item itemScript = obj.GetComponent<Item>();
        if (itemScript == null || !itemScript.inSlot) return;

        // 슬롯 정보 초기화
        ItemInSlot = null;

        // 부모 관계 제거
        obj.transform.parent = null;

        // 물리 엔진 활성화
        Rigidbody objRigidbody = obj.GetComponent<Rigidbody>();
        if (objRigidbody != null)
        {
            objRigidbody.isKinematic = false;
        }

        // 아이템 상태 초기화
        itemScript.inSlot = false;
        itemScript.CurrentSlot = null;

        // 슬롯 색상 초기화
        ResetColor();
    }

    // 슬롯 색상 초기화
    public void ResetColor()
    {
        slotImage.color = originalColor;
    }
}
