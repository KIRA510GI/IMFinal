using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ResizeObjectOnSocket : MonoBehaviour
{
    public Vector3 resizedScale = new Vector3(0.5f, 0.5f, 0.5f); // 상자 안에서의 크기
    private Vector3 originalScale; // 원래 크기 저장

    private void Start()
    {
        // Socket Interactor의 이벤트 등록
        XRSocketInteractor socketInteractor = GetComponent<XRSocketInteractor>();
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.AddListener(OnObjectInserted);
            socketInteractor.selectExited.AddListener(OnObjectRemoved);
        }
    }
    private void OnObjectInserted(SelectEnterEventArgs args)
    {
        Transform interactableObject = args.interactableObject.transform;
        Debug.Log("Object Inserted! Original Scale: " + interactableObject.localScale);
        
        // 부모의 영향을 받지 않도록 직접 localScale을 설정
        interactableObject.localScale = resizedScale;
        Debug.Log("New Scale: " + interactableObject.localScale);

        // Collider 크기 리프레시
        Collider objCollider = interactableObject.GetComponent<Collider>();
        if (objCollider != null)
        {
            objCollider.enabled = false;  // 임시로 비활성화
            objCollider.enabled = true;   // 크기 변경 후 Collider 리프레시
        }
    }
    private void OnObjectRemoved(SelectExitEventArgs args)
    {
        // 상자 밖으로 오브젝트가 나갈 때 크기 복원
        Transform interactableObject = args.interactableObject.transform;
        interactableObject.localScale = originalScale; // 원래 크기 복원
    }
}
