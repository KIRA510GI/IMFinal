using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketScaleHandler : MonoBehaviour
{
    private XRSocketInteractor _socketInteractor; // Socket Interactor 참조
    private Vector3 _originalScale;               // 오브젝트의 원래 크기
    public float scaleMultiplier = 0.5f;         // 축소 비율
    private void Awake()
    {
        // XR Socket Interactor 컴포넌트를 가져오기
        _socketInteractor = GetComponent<XRSocketInteractor>();

        // Interaction 이벤트 등록
        _socketInteractor.selectEntered.AddListener(OnObjectPlaced);
        _socketInteractor.selectExited.AddListener(OnObjectRemoved);

        Debug.Log($"[SocketScaleHandler] Initialized on {gameObject.name}");
    }

    private void OnDestroy()
    {
        // Interaction 이벤트 해제
        _socketInteractor.selectEntered.RemoveListener(OnObjectPlaced);
        _socketInteractor.selectExited.RemoveListener(OnObjectRemoved);

        Debug.Log($"[SocketScaleHandler] Destroyed on {gameObject.name}");
    }

    private void OnObjectPlaced(SelectEnterEventArgs args)
    {
        // 들어온 오브젝트의 Transform 가져오기
        Transform placedObject = args.interactableObject.transform;

        // 원래 크기 저장
        _originalScale = placedObject.localScale;

        // 크기를 축소
        placedObject.localScale = _originalScale * scaleMultiplier;

        Debug.Log($"[SocketScaleHandler] Object {placedObject.name} placed in socket {gameObject.name}. " +
                  $"Original Scale: {_originalScale}, New Scale: {placedObject.localScale}");
    }

    private void OnObjectRemoved(SelectExitEventArgs args)
    {
        // 나간 오브젝트의 Transform 가져오기
        Transform removedObject = args.interactableObject.transform;

        // 크기를 원래대로 복구
        removedObject.localScale = _originalScale;

        Debug.Log($"[SocketScaleHandler] Object {removedObject.name} removed from socket {gameObject.name}. " +
                  $"Restored Scale: {_originalScale}");
    }
}