using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerSpeed : MonoBehaviour
{
    [SerializeField]
    private InputActionProperty moveAction;
    [SerializeField]
    private float moveSpeed = 3.0f;

void Update()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0, input.y).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

}
