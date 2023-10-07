using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermitMiniGameController : BasicMiniGameController
{
    [SerializeField] private Transform lightSource;
    [SerializeField] private Camera targetCam;

    private float depth;
    private float ratio;
    private Camera mainCam;

    void Awake(){
        mainCam = Camera.main;
        ratio = (targetCam.pixelWidth+0f)/(mainCam.pixelWidth+0f);
    }
    void OnEnable(){
        depth = targetCam.WorldToScreenPoint(lightSource.position).z;
    }
    public override void UpdateMiniGame(Vector3 pointerPos)
    {
        pointerPos.z = depth;
        pointerPos.x *= ratio;
        pointerPos.y *= ratio;
        Vector3 pos = targetCam.ScreenToWorldPoint(pointerPos);
        pos.y = lightSource.position.y;
        lightSource.position = pos;
    }
}
