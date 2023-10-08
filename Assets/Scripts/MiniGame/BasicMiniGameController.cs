using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicMiniGameController : MonoBehaviour
{
    [SerializeField] private CardType miniGameType;
    [SerializeField] protected Camera targetCamera;
    public bool m_updating = false;
    protected float ratio;
    protected Camera mainCam;
    public void StartMiniGame(){
        m_updating = true;
        this.enabled = true;

        OnStart();
    }
    public void SetUp(Camera _targetCam){
        targetCamera = _targetCam;
        mainCam = Camera.main;
        ratio = (targetCamera.pixelWidth+0f)/(mainCam.pixelWidth+0f);
    }
    public abstract void UpdateMiniGame(Vector3 pointerScreenPos);
    protected virtual void OnStart(){}
    protected Vector3 GetWorldPointerPos(Vector3 pointerScreenPos, float depth){
        pointerScreenPos.z = depth;
        pointerScreenPos.x *= ratio;
        pointerScreenPos.y *= ratio;
        return targetCamera.ScreenToWorldPoint(pointerScreenPos);
    }
}
