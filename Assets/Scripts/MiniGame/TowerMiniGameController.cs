using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMiniGameController : BasicMiniGameController
{
[Header("Person Control")]
    [SerializeField] private Transform personTrans;
    [SerializeField] private Transform personRotateRoot;
    [SerializeField] private float distanceToAngle;
[Header("Person Sprite")]
    [SerializeField] private SpriteRenderer personRenderer;
    [SerializeField] private Sprite[] personSprite;

    [SerializeField] private int side = 1;
    private float depth;
    private CoroutineExcuter flipExcuter;

    void Awake()=>SetUp(targetCamera);
    void Start(){
        flipExcuter = new CoroutineExcuter(this);
    }
    void OnEnable(){
        depth = targetCamera.WorldToScreenPoint(personTrans.position).z;
    }
    public override void UpdateMiniGame(Vector3 pointerScreenPos)
    {
        Vector3 pos = GetWorldPointerPos(pointerScreenPos, depth);
        float diff = pos.z - personTrans.position.z;
        pos.y = personTrans.position.y;
        pos.z = personTrans.position.z;
        personTrans.position = pos;

        personRotateRoot.localRotation = Quaternion.Euler(0,0,distanceToAngle*diff);

        if(side > 0 && personTrans.localPosition.x < 0){
            Debug.Log("Flip");
            side = -1;
            flipExcuter.Excute(coroutineFlipSize(-90, 0.5f));
        }
        if(side < 0 && personTrans.localPosition.x > 0){
            side = 1;
            flipExcuter.Excute(coroutineFlipSize(90, 0.5f));
        }
    }
    IEnumerator coroutineFlipSize(float targetAngle, float duration){
        float currentAngle = personTrans.localEulerAngles.x;
        yield return new WaitForLoop(duration, (t)=>{
            float angle = Mathf.Lerp(currentAngle, targetAngle, EasingFunc.Easing.QuadEaseOut(t));
            personTrans.localRotation = Quaternion.Euler(angle, 90, 90);
        });
    }
}
