using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VisionPP : MonoBehaviour
{
    [SerializeField] private PostProcessVolume ppVolume;
    private CoroutineExcuter PPFader;
    void Start(){
        PPFader = new CoroutineExcuter(this);
    }
    void OnEnable(){
        EventHandler.E_OnFoundVision += FoundVision;
        EventHandler.E_OnLostVision += LostVision;
    }
    void OnDisable(){
        EventHandler.E_OnFoundVision -= FoundVision;
        EventHandler.E_OnLostVision -= LostVision;
    }
    void FoundVision(){
        PPFader.Excute(coroutineFadePP(1, 0.25f));
    }
    void LostVision(){
        PPFader.Excute(coroutineFadePP(0, 0.25f));
    }
    IEnumerator coroutineFadePP(float targetWeight, float duration){
        float initWeight = ppVolume.weight;
        yield return new WaitForLoop(duration, (t)=>{
            ppVolume.weight = Mathf.Lerp(initWeight, targetWeight, EasingFunc.Easing.SmoothInOut(t));
        });
    }
}
