using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicMiniGameController : MonoBehaviour
{
    [SerializeField] private CardType miniGameType;
    
    public abstract void UpdateMiniGame(Vector3 pointerScreenPos);
    protected Vector3 GetWorldPointerPos(Camera cam, Vector3 pointerScreenPos, float depth){
        pointerScreenPos.z = depth;
        return cam.WorldToScreenPoint(pointerScreenPos);
    }
}
