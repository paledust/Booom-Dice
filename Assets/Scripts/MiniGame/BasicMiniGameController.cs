using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicMiniGameController : MonoBehaviour
{
    [SerializeField] private CardType miniGameType;
    [SerializeField] protected Camera targetCamera;

    protected float depth;
    protected float ratio;
    protected Camera mainCam;

    public void SetUp(Camera _targetCam){
        mainCam = Camera.main;
        ratio = (targetCamera.pixelWidth+0f)/(mainCam.pixelWidth+0f);
        targetCamera = _targetCam;
    }
    public abstract void UpdateMiniGame(Vector3 pointerScreenPos);
    protected Vector3 GetWorldPointerPos(Vector3 pointerScreenPos){
        pointerScreenPos.z = depth;
        pointerScreenPos.x *= ratio;
        pointerScreenPos.y *= ratio;
        return targetCamera.ScreenToWorldPoint(pointerScreenPos);
    }
}
