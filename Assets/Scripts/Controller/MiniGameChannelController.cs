using System.Collections;
using System.Collections.Generic;
using SimpleAudioSystem;
using UnityEngine;

[System.Serializable]
public class ChannelData{
    public float sinOffset;
    public float sinWeight;
    public string channelWeightName;
    public string channelMuteName;
    public string mixerValueName;
    public CoroutineExcuter channelFader;
    public bool isShowing = false;
}
public class MiniGameChannelController : MonoBehaviour
{
    [SerializeField] private Vector2 sinFadeRange;
    [SerializeField] private float sinFadeSpeed = 0.25f;
    [Range(0, 1)] public float[] channelWeight;
    [SerializeField] private ChannelData[] channelDatas;
[Header("Fade Timing")]
    [SerializeField] private float channelFadeInTime = 0.2f;
    [SerializeField] private float channelStayTime = 2;
    [SerializeField] private float channelLerpToSinTime = 4;
[Space(20), Header("Information")]
    [SerializeField] private float[] realTimeWeight;

    void OnEnable(){
        EventHandler.E_OnFoundVision += FoundVisionHandler;
        EventHandler.E_OnLostVision +=  LostVisionHandler;
    }
    void OnDisable(){
        EventHandler.E_OnFoundVision -= FoundVisionHandler;
        EventHandler.E_OnLostVision -= LostVisionHandler;
    }
    void FoundVisionHandler(int visionIndex){
        for(int i=0;i<channelDatas.Length; i++){
            if(channelDatas[i].isShowing){
                if(i!=visionIndex){
                    channelDatas[i].channelFader.Excute(coroutineFadeChannelWeight(i, 0, 0.5f));
                    Shader.SetGlobalFloat(channelDatas[i].channelMuteName, 1);
                }
                else{
                    channelDatas[i].sinWeight = 0;
                }
            }
        }
    }
    void LostVisionHandler(int visionIndex){
        for(int i=0;i<channelDatas.Length; i++){
            if(channelDatas[i].isShowing){
                if(i!=visionIndex){
                    channelDatas[i].channelFader.Excute(coroutineFadeChannelWeight(i, 1, 0.5f));
                    Shader.SetGlobalFloat(channelDatas[i].channelMuteName, 0);
                }
                else{
                    channelDatas[i].sinWeight = 1;
                }
            }
        }
    }
    void Start(){
        for(int i=0; i<channelDatas.Length; i++){
            channelDatas[i].channelFader = new CoroutineExcuter(this);
        }
        realTimeWeight = new float[channelWeight.Length];
    }
    void Update(){
        for(int i=0; i<channelDatas.Length; i++){
        //Update Weight
            float t = Time.time * sinFadeSpeed + channelDatas[i].sinOffset * Mathf.PI;
            t = Mathf.SmoothStep(sinFadeRange.x, sinFadeRange.y, Mathf.Sin(t)*0.5f + 0.5f);
            t = Mathf.Lerp(1, t, channelDatas[i].sinWeight);
            realTimeWeight[i] = t * channelWeight[i];
        //Assign Value Based On Weight
            Shader.SetGlobalFloat(channelDatas[i].channelWeightName, realTimeWeight[i]);
            AudioManager.Instance.SetMixerValue(channelDatas[i].mixerValueName, Mathf.Lerp(-25, 0, realTimeWeight[i]));
        }
    }
    public void FadeInChannel(int targetChannel){
        StartCoroutine(coroutineFadeInChannel(targetChannel, channelFadeInTime, channelLerpToSinTime));
    }
    IEnumerator coroutineFadeInChannel(int targetChannel, float duration, float toSinDuration){
        channelDatas[targetChannel].isShowing = true;

        switch(targetChannel){
            case 0:
                channelDatas[0].channelFader.Excute(coroutineFadeChannelWeight(0, 1, duration));
                break;
            case 1:
                channelDatas[0].channelFader.Excute(coroutineFadeChannelWeight(0, 0, duration));
                channelDatas[1].channelFader.Excute(coroutineFadeChannelWeight(1, 1, duration));
                break;
            case 2:
                channelDatas[0].channelFader.Excute(coroutineFadeChannelWeight(0, 0, duration));
                channelDatas[1].channelFader.Excute(coroutineFadeChannelWeight(1, 0, duration));
                channelDatas[2].channelFader.Excute(coroutineFadeChannelWeight(2, 1, duration));
                break;
        }

        yield return new WaitForSeconds(channelStayTime+duration);

        switch(targetChannel){
            case 0:
                channelDatas[0].channelFader.Excute(coroutineFadeChannelWeight(0, 1, toSinDuration));
                break;
            case 1:
                channelDatas[0].channelFader.Excute(coroutineFadeChannelWeight(0, 1, toSinDuration));
                channelDatas[1].channelFader.Excute(coroutineFadeChannelWeight(1, 1, toSinDuration));
                break;
            case 2:
                channelDatas[0].channelFader.Excute(coroutineFadeChannelWeight(0, 1, toSinDuration));
                channelDatas[1].channelFader.Excute(coroutineFadeChannelWeight(1, 1, toSinDuration));
                channelDatas[2].channelFader.Excute(coroutineFadeChannelWeight(2, 1, toSinDuration));
                break;            
        }

        yield return new WaitForLoop(toSinDuration, (t)=>{
            channelDatas[targetChannel].sinWeight = Mathf.Lerp(0, 1, t);
        });
    }
    IEnumerator coroutineFadeChannelWeight(int targetChannel, float targetWeights, float duration){
        float initWeight = channelWeight[targetChannel];
        yield return new WaitForLoop(duration, (t)=>{
            channelWeight[targetChannel] = Mathf.Lerp(initWeight, targetWeights, t);
        });
    }
}
