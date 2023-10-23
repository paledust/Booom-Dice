using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameText : MonoBehaviour
{
    public ParticleSystem puffParticle;
    public TextMeshProUGUI text;
    public int order;
    private CoroutineExcuter textFader;
    void Awake(){
        textFader = new CoroutineExcuter(this);
        text.text = "--";
        text.fontMaterial.SetFloat("_OutlineSoftness", 0.5f);
    }
    public void FadeInText(){
        puffParticle.transform.position = transform.position+Vector3.forward*0.1f;
        puffParticle.Play();
        textFader.Excute(CommonCoroutine.CoroutineFadeText(text, 1f, 0.75f, EasingFunc.Easing.FunctionType.QuadEaseOut));
    }
    public void FadeOutText(){
        textFader.Excute(CommonCoroutine.CoroutineFadeText(text, 0, 0.5f, EasingFunc.Easing.FunctionType.CubicEaseIn));
    }
    public void FadeOutText(float duration){
        textFader.Excute(CommonCoroutine.CoroutineFadeText(text, 0, duration, EasingFunc.Easing.FunctionType.CubicEaseIn));
    }
}
