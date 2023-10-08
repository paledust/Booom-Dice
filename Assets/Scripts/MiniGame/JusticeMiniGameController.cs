using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.InputSystem.XR;

public class JusticeMiniGameController : BasicMiniGameController
{
    [SerializeField] private Transform eyeCenterTrans;
    [SerializeField] private Transform eyeTrans;
    [SerializeField] private Animator eyeAnimator;
    [SerializeField] private float eyeRadius;
    [SerializeField] private float eyeSpeedLerp = 10;
[Header("Look At Object")]
    [SerializeField] private BalancingHand balancingHand;

    private float depth;
    private Lookable currentLookable;
    private const string onLookTriggerName = "OnLook";
    private const string exitLookTriggerName = "ExitLook";
    private const string blinkTriggerName = "Blink";

    void OnEnable(){
        depth = targetCamera.WorldToScreenPoint(eyeCenterTrans.position).z;
        EventHandler.E_OnSpotLookable += SpotLookableHandler;
    }
    void OnDisable(){
        EventHandler.E_OnSpotLookable -= SpotLookableHandler;
    }
    public override void UpdateMiniGame(Vector3 pointerScreenPos)
    {
    //Eye Control
        Vector3 pos = GetWorldPointerPos(pointerScreenPos, depth);
        pos.y = eyeCenterTrans.position.y;

        Vector3 diff = pos - eyeCenterTrans.position;
        diff = Vector3.ClampMagnitude(diff, eyeRadius);

        pos.x = diff.x;
        pos.y = diff.z;
        pos.z = 0;
        eyeTrans.localPosition = Vector3.Lerp(eyeTrans.localPosition, pos, Time.deltaTime * eyeSpeedLerp);

    //Object Looking At
        pointerScreenPos.x *= ratio;
        pointerScreenPos.y *= ratio;
        Ray ray = targetCamera.ScreenPointToRay(pointerScreenPos);
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)){
            Lookable hit_lookable = hit.collider.GetComponent<Lookable>();
            if(hit_lookable!=null){
                if(currentLookable != hit_lookable) {
                    if(currentLookable!=null) currentLookable.OnNotLooked();
                    else{
                        eyeTrans.localScale = Vector3.one * 0.02f;
                        eyeAnimator.SetTrigger(onLookTriggerName);
                    }
                    currentLookable = hit_lookable;
                    currentLookable.OnLookAt();
                }
            }
            else{
                ClearLookable();
            }
        }
        else{
            ClearLookable();
        }
    }
    public void SpotLookableHandler(LookableType lookableType){
        eyeAnimator.SetTrigger(blinkTriggerName);
        switch(lookableType){
            case LookableType.Heart:
                balancingHand.AddToLeftHand(0.3f);
                break;
            case LookableType.Feather:
                balancingHand.AddToRightHand(0.3f);
                break;
        }
    }
    void ClearLookable()
    {
        if(currentLookable != null){
            currentLookable.OnNotLooked();
            currentLookable = null;
            eyeTrans.localScale = Vector3.one * 0.015f;
            eyeAnimator.SetTrigger(exitLookTriggerName);
        }
    }
    void OnDrawGizmosSelected(){
        Gizmos.matrix = eyeCenterTrans.localToWorldMatrix;
        DebugExtension.DrawCircle(Vector3.zero, Vector3.forward, Color.green, eyeRadius);
    }
}
