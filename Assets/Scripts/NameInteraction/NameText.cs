using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public int order;
    private CoroutineExcuter textFader;
    void Awake(){
        textFader = new CoroutineExcuter(this);
    }
    public void FadeInText(){
        textFader.Excute(CommonCoroutine.CoroutineFadeText(text, 1f, 1.5f, EasingFunc.Easing.FunctionType.QuadEaseOut));
    }
    public void FadeOutText(){
        textFader.Excute(CommonCoroutine.CoroutineFadeText(text, 0, 0.5f, EasingFunc.Easing.FunctionType.CubicEaseIn));
    }
    public void FadeOutText(float duration){
        textFader.Excute(CommonCoroutine.CoroutineFadeText(text, 0, duration, EasingFunc.Easing.FunctionType.CubicEaseIn));
    }
}