using System.Collections;
using System.Collections.Generic;
using SimpleAudioSystem;
using UnityEngine;

[System.Serializable]
public class ChannelData{
    public float sinOffset;
    public float sinWeight;
    public string channelWeightName;
    public string mixerValueName;
}
public class MiniGameChannelController : MonoBehaviour
{
    [SerializeField] private Vector2 sinFadeRange;
    [SerializeField] private float sinFadeSpeed = 0.25f;
    [SerializeField, Range(0, 1)] private float[] channelWeight;
    [SerializeField] private ChannelData[] channelDatas;
[Header("Fade Timing")]
    [SerializeField] private float channelFadeInTime = 0.2f;
    [SerializeField] private float channelLerpToSinTime = 4;
[Space(20), Header("Information")]
    [SerializeField] private float[] realTimeWeight;

    private CoroutineExcuter channelFader;

    void Start(){
        channelFader = new CoroutineExcuter(this);
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
            AudioManager.Instance.SetMixerValue(channelDatas[i].mixerValueName, Mathf.Lerp(-80, 0, realTimeWeight[i]));
        }
    }
    public void FadeInChannel(int targetChannel){
        StartCoroutine(coroutineFadeInChannel(targetChannel, channelFadeInTime, channelLerpToSinTime));
    }
    IEnumerator coroutineFadeInChannel(int targetChannel, float duration, float toSinDuration){
        // yield return new WaitForLoop(duration, (t)=>{
        //     channelWeight[targetChannel] = Mathf.Lerp(0, 1, EasingFunc.Easing.SmoothInOut(t));
        // });
        // channelFader.Excute()

        yield return new WaitForSeconds(2f);

        yield return new WaitForLoop(toSinDuration, (t)=>{
            channelDatas[targetChannel].sinWeight = Mathf.Lerp(0, 1, t);
        });
    }
    IEnumerator coroutineFadeChannelWeight(float[] channelweights, float duration){
        float[] currentWeights = channelweights;
        yield return new WaitForLoop(duration, (t)=>{
            for(int i=0; i<channelweights.Length; i++){
                // channelweights[i] = Mathf.Lerp()
            }
        });
    }
}
