using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
[Header("Hand Control")]
    [SerializeField] private HandController _hand;
    [SerializeField] private List<BasicMiniGameController> miniGames;
    Vector3 pointerPos;

    void Update(){
        _hand.Hand_Update(pointerPos);
        foreach(BasicMiniGameController miniGame in miniGames){
            if(miniGame!=null){
                miniGame.UpdateMiniGame(pointerPos);
            }
        }
    }
    void OnPointer(InputValue value){
        pointerPos = value.Get<Vector2>();
    }
    void OnInteract(InputValue value){
        _hand.Hand_Interact(value.isPressed);
    }
}
