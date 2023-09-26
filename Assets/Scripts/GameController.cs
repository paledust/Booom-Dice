using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
[Header("Hand Control")]
    [SerializeField] private HandController _hand;

    Vector3 pointerPos;

    void Update(){
        _hand.UpdateInput(pointerPos);
    }
    void OnPointer(InputValue value){
        pointerPos = value.Get<Vector2>();
    }
}
