using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextSphere : MonoBehaviour
{
    [SerializeField] private SphereState sphereState;
    [SerializeField] private Vector2 idleSpeed;
    [SerializeField] private TextMeshPro[] texts;
    [SerializeField] private float radius;
    [SerializeField] private float textScale;
    [SerializeField] private ConditionDetector conditionDetector;
    
    private Rigidbody followRigid;

    private static readonly Vector3[] textPosVecs = new Vector3[]{new Vector3(0,0,1.3333f), new Vector3(0.4714f, -0.8164f, 0), new Vector3(-0.9428f, 0, 0), new Vector3(0.4714f, 0.8164f, 0)};

    void Start(){
        conditionDetector.OnComplete(OnCompleteHandler);
    }
    void Update()
    {
        switch(sphereState){
            case SphereState.Idle:
                transform.rotation *= Quaternion.Euler(Mathf.PerlinNoise(0.2f,0.5421f+Time.time)*idleSpeed.x*Time.deltaTime, Mathf.PerlinNoise(0.4212f,Time.time+0.3221f)*idleSpeed.y*Time.deltaTime, 0);
                break;
            default:
                transform.rotation = Quaternion.Slerp(transform.rotation, followRigid.rotation, Time.deltaTime*5);
                break;
        }
        for(int i=0; i<texts.Length; i++){
            texts[i].transform.position = transform.position + transform.rotation * textPosVecs[i]*radius;
            texts[i].transform.localScale = Vector3.one*textScale;
        }

        if(sphereState == SphereState.DetectingWords){
            conditionDetector.DetectUpdate(followRigid.angularVelocity.magnitude<=0.01f);
        }
    }
    public void ExpandSphere(string[] words){
        this.enabled = true;
        for(int i=0; i<texts.Length; i++){
            texts[i].text = words[i];
        }
        StartCoroutine(coroutineExpandSphere(1));
    }
    public void DetectingWords(){
        sphereState = SphereState.DetectingWords;
    }
    public void FollowDice(Transform diceTrans){
        followRigid = diceTrans.GetComponent<Rigidbody>();
        sphereState = SphereState.Follow;
    }
    void OnCompleteHandler(){
        TextMeshPro topText = texts[0];
        for(int i=0; i<texts.Length; i++){
            if(texts[i].transform.position.y>topText.transform.position.y) topText = texts[i];
        }

        GameController.Instance.FindTheWords(topText);
        StartCoroutine(coroutineShrinkSphere(1));
    }
    IEnumerator coroutineExpandSphere(float duration){
        yield return new WaitForLoop(duration, (t)=>{
            radius = Mathf.LerpUnclamped(0, 4, EasingFunc.Easing.BackEaseOut(t));
            textScale = Mathf.LerpUnclamped(0, 1, EasingFunc.Easing.BackEaseOut(t));
        });
    }
    IEnumerator coroutineShrinkSphere(float duration){
        yield return new WaitForLoop(duration, (t)=>{
            radius = Mathf.LerpUnclamped(4, 0, EasingFunc.Easing.BackEaseIn(t));
            textScale = Mathf.LerpUnclamped(1, 0, EasingFunc.Easing.BackEaseIn(t));
        });
        for(int i=0; i<texts.Length; i++){
            texts[i].transform.position = transform.position;
            texts[i].transform.localScale = Vector3.zero;
        }
        sphereState = SphereState.Idle;
        this.enabled = false;
    }
}
