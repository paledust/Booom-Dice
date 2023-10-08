using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class JusticeMiniGameController : BasicMiniGameController
{
    [SerializeField] private Transform eyeCenterTrans;
    [SerializeField] private Transform eyeTrans;
    [SerializeField] private float eyeRadius;
    [SerializeField] private float eyeSpeedLerp = 10;
    private float depth;
    void Awake()=>SetUp(targetCamera);
    void OnEnable(){
        depth = targetCamera.WorldToScreenPoint(eyeCenterTrans.position).z;
    }
    public override void UpdateMiniGame(Vector3 pointerScreenPos)
    {
        Vector3 pos = GetWorldPointerPos(pointerScreenPos, depth);
        pos.y = eyeCenterTrans.position.y;

        Vector3 diff = pos - eyeCenterTrans.position;
        diff = Vector3.ClampMagnitude(diff, eyeRadius);

        pos.x = diff.x;
        pos.y = diff.z;
        pos.z = 0;
        eyeTrans.localPosition = Vector3.Lerp(eyeTrans.localPosition, pos, Time.deltaTime * eyeSpeedLerp);
    }
    void OnDrawGizmosSelected(){
        Gizmos.matrix = eyeCenterTrans.localToWorldMatrix;
        DebugExtension.DrawCircle(Vector3.zero, Vector3.forward, Color.green, eyeRadius);
    }
}
