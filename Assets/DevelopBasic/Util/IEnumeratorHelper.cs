using System.Collections;
using System.Collections.Generic;
using EasingFunc;
using TMPro;
using UnityEngine;

public static class CommonCoroutine
{
    public static IEnumerator CoroutineFadeText(TextMeshProUGUI text, float targetAlpha, float duration, Easing.FunctionType easeType=Easing.FunctionType.QuadEaseOut){
        Color initColor = text.color;
        Color targetColor = initColor;
        targetColor.a = targetAlpha;

        string outlineSoftnessName = "_OutlineSoftness";
        float initSoftness = text.fontMaterial.GetFloat(outlineSoftnessName);
        float targetSoftness = 1-targetAlpha;

        var easeFunc = Easing.GetFunctionWithTypeEnum(easeType);
        yield return new WaitForLoop(duration, (t)=>{
            text.fontMaterial.SetFloat(outlineSoftnessName, Mathf.Lerp(initSoftness, targetSoftness, easeFunc(Mathf.Clamp01(t*5))));
            text.color = Color.Lerp(initColor, targetColor, easeFunc(t));
        });
    }    
}

public class CoroutineExcuter
{
    MonoBehaviour initiator;
    IEnumerator coroutine;
    public CoroutineExcuter(MonoBehaviour _context)=>initiator = _context;
    public void Excute(IEnumerator go){
        if(coroutine!=null) initiator.StopCoroutine(coroutine);
        coroutine = go;
        initiator.StartCoroutine(go);
    }
    public void Abort(){
        if(coroutine!=null) initiator.StopCoroutine(coroutine);
    }
}

public class WaitForLoop: IEnumerator
{
    public delegate void LoopMain(float t);
    private IEnumerator m_coroutine;

    public WaitForLoop(float _duration, LoopMain _go){
        m_coroutine = ForLoopCoroutine(_duration, _go);
    }

    public object Current{get{return m_coroutine.Current;}}
    public bool MoveNext(){return m_coroutine.MoveNext();}
    public void Reset()=>m_coroutine.Reset();
    
    public static IEnumerator ForLoopCoroutine(float duration, LoopMain go){
        float speed = 1f/duration;
        for(float t=0; t<1; t+=Time.deltaTime*speed){
            go(t);
            yield return null;
        }
        go(1);
    }
}