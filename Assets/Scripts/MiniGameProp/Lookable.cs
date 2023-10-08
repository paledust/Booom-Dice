using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lookable : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private LookableType lookableType;
    [SerializeField] private float fadeTime = 0.25f;
    [SerializeField] private ConditionDetector conditionDetector;
    private CoroutineExcuter spriteFader;
    private bool isLooked = false;
    void Start(){
        spriteFader = new CoroutineExcuter(this);
        conditionDetector.OnReset(OnReset);
        conditionDetector.OnComplete(OnComplete);
    }
    public void OnLookAt(){
        isLooked = true;

    }
    public void OnNotLooked(){
        isLooked = false;
    }
    void Update(){
        conditionDetector.DetectUpdate(isLooked);
    }
    void OnReset(){}
    void OnComplete(){}
    IEnumerator coroutineFadeSprite(Color targetColor, float duration){

    }
}