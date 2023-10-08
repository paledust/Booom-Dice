using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JusticeMiniGameController : BasicMiniGameController
{
    [SerializeField] private Transform eyeCenterTrans;
    private float depth;
    void Awake()=>SetUp(targetCamera);
    void OnEnable(){
        depth = targetCamera.WorldToScreenPoint(eyeCenterTrans.position).z;
    }
    public override void UpdateMiniGame(Vector3 pointerScreenPos)
    {
    }
}
