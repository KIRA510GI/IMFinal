using UnityEngine;
using UnityEngine.InputSystem;

public class RemovefromSlot : MonoBehaviour
{
    public InputActionProperty grabAction;
    public void RemoveItem(GameObject obj)
    {
        if (gameObject.GetComponent<Item>() == null) return;
        if (gameObject.GetComponent<Item>().inSlot && grabAction.action.ReadValue<float>() > 0.1f)
        {
            gameObject.GetComponentInParent<InventorySlot>().ItemInSlot = null;
            gameObject.transform.parent = null;
            gameObject.GetComponent<Item>().inSlot = false;
            gameObject.GetComponent<Item>().CurrentSlot.ResetColor();
            gameObject.GetComponent<Item>().CurrentSlot = null;
        }
    }
}
