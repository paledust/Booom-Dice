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
    public static IEnumerator CoroutineFadeText(TextMeshPro text, float targetAlpha, float duration, Easing.FunctionType easeType=Easing.FunctionType.QuadEaseOut){
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
    public static IEnumerator CoroutineDissolveSprite(SpriteRenderer m_sprite, float initRadius, float targetRadius, float duration, Easing.FunctionType easeType=Easing.FunctionType.QuadEaseOut){
        var easeFunc = Easing.GetFunctionWithTypeEnum(easeType);
        yield return new WaitForLoop(duration, (t)=>{
            m_sprite.material.SetFloat(GameController.DissolveRadiusName, Mathf.Lerp(initRadius, targetRadius, easeFunc(t)));
        });
    }
    public static IEnumerator CoroutineFadeSprite(SpriteRenderer m_sprite, float targetAlpha, float duration){
        Color targetColor = m_sprite.color;
        Color initColor = m_sprite.color;
        targetColor.a = targetAlpha;
        yield return new WaitForLoop(duration, (t)=>{
            m_sprite.color = Color.Lerp(initColor, targetColor, Easing.SmoothInOut(t));
        });
    }
    public static IEnumerator CoroutineSetTrans(Transform trans, Vector3 targetPos, Quaternion targetRot, bool isLocal, float duration, Easing.FunctionType easeType = Easing.FunctionType.QuadEaseOut){
        Vector3 initPos = isLocal?trans.localPosition:trans.position;
        Quaternion initRot = isLocal?trans.localRotation:trans.rotation;

        var easeFunc = Easing.GetFunctionWithTypeEnum(easeType);
        yield return new WaitForLoop(duration, (t)=>{
            if(isLocal){
                trans.localPosition = Vector3.LerpUnclamped(initPos, targetPos, easeFunc(t));
                trans.localRotation = Quaternion.LerpUnclamped(initRot, targetRot, easeFunc(t));
            }
            else{
                trans.position = Vector3.LerpUnclamped(initPos, targetPos, easeFunc(t));
                trans.rotation = Quaternion.LerpUnclamped(initRot, targetRot, easeFunc(t));
            }
        });
    }
    public static IEnumerator CoroutineChangeTransSize(Transform trans, Vector3 targetSize, float duration){
        Vector3 initSize = trans.localScale;
        yield return new WaitForLoop(duration, (t)=>{
            trans.localScale = Vector3.Lerp(initSize, targetSize, Easing.SmoothInOut(t));
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