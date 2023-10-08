using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceShadow : MonoBehaviour
{
    [SerializeField] private Image shadowImage;
    [SerializeField] private float dissolveInTime = 4f;
    [SerializeField] private float dissolveOutTime = 0.25f;
    [SerializeField] private Color finalColor;
    private CoroutineExcuter imageDissolver;
    private Material m_mat;

    private const string DissolveRadiusName = "_DissolveRadius";

    void Awake(){
        m_mat = Instantiate(shadowImage.material);
        shadowImage.material = m_mat;
        imageDissolver = new CoroutineExcuter(this);
    }
    void OnDestroy(){
        Destroy(m_mat);
    }
    public void BrightFace(){
        StartCoroutine(coroutineFadeSprite(0.5f, finalColor));
    }
    public void DissolveInShadow(){
        imageDissolver.Excute(coroutineDissolveShadow(dissolveInTime, 0f, EasingFunc.Easing.FunctionType.QuadEaseOut));
    }
    public void DissolveOutShadow(){
        imageDissolver.Excute(coroutineDissolveShadow(dissolveOutTime, 7f));
    }
    IEnumerator coroutineDissolveShadow(float duration, float targetRadius, EasingFunc.Easing.FunctionType easeType=EasingFunc.Easing.FunctionType.Linear){
        var easeFunc = EasingFunc.Easing.GetFunctionWithTypeEnum(easeType);
        float initRadius = m_mat.GetFloat(DissolveRadiusName);
        yield return new WaitForLoop(duration,(t)=>{
            m_mat.SetFloat(DissolveRadiusName, Mathf.Lerp(initRadius, targetRadius, easeFunc(t)));
        });
    }
    IEnumerator coroutineFadeSprite(float duraiton, Color targetColor){
        Color initColor = shadowImage.color;
        yield return new WaitForLoop(duraiton, (t)=>{
            shadowImage.color = Color.Lerp(initColor, targetColor, EasingFunc.Easing.SmoothInOut(t));
        });
    }
}
