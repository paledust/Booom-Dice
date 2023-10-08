using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lookable : MonoBehaviour
{
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
    public void OnReset(Vector3 position){transform.position = position;}
    void Update(){
        conditionDetector.DetectUpdate(isLooked);
    }
    void OnComplete(){}
    IEnumerator coroutineFadeSprite(Color targetColor, float duration){
        Color startColor = spriteRenderer.color;
        yield return new WaitForLoop(duration, (t)=>{
            spriteRenderer.color = Color.Lerp(startColor, targetColor, EasingFunc.Easing.SmoothInOut(t));
        });
    }
}