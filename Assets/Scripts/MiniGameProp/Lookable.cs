using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lookable : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private LookableType lookableType;
    [SerializeField] private float fadeTime = 0.25f;
    [SerializeField] private ConditionDetector conditionDetector;
    [SerializeField] private Color initColor;
    private CoroutineExcuter spriteFader;
    private bool isLooked = false;
    void Start(){
        spriteFader = new CoroutineExcuter(this);
        conditionDetector.OnComplete(OnComplete);
    }
    public void OnLookAt(){
        isLooked = true;
        spriteFader.Excute(coroutineFadeSprite(Color.white, fadeTime));
    }
    public void OnNotLooked(){
        isLooked = false;
        spriteFader.Excute(coroutineFadeSprite(initColor, fadeTime));
    }
    public void OnReset(Vector3 position){
        transform.position = position;
        transform.localRotation = Quaternion.Euler(90,0,Random.Range(0,360));
    }
    void Update(){
        conditionDetector.DetectUpdate(isLooked);
        transform.position += Vector3.back * moveSpeed * Time.deltaTime;
    }
    void OnComplete(){
        StartCoroutine(coroutineShrink());
    }
    IEnumerator coroutineFadeSprite(Color targetColor, float duration){
        Color startColor = spriteRenderer.color;
        yield return new WaitForLoop(duration, (t)=>{
            spriteRenderer.color = Color.Lerp(startColor, targetColor, EasingFunc.Easing.SmoothInOut(t));
        });
    }
    IEnumerator coroutineShrink(){
        EventHandler.Call_OnSpotLookable(lookableType);

        Vector3 initSize = transform.localScale;
        yield return new WaitForLoop(0.5f, (t)=>{
            transform.localScale = Vector3.LerpUnclamped(initSize, Vector3.zero, EasingFunc.Easing.QuadEaseOut(t));
        });
        gameObject.SetActive(false);
        transform.localScale = initSize;
    }
}