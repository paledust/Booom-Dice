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
[Space(20), Header("Information")]
    [SerializeField] private float[] realTimeWeight;
    void Start(){
        realTimeWeight = new float[channelWeight.Length];
    }
    void Update(){
        for(int i=0; i<channelDatas.Length; i++){
        //Update Weight
            // float t = Time.time * sinFadeSpeed + channelDatas[i].sinOffset * Mathf.PI;
            // t = Mathf.SmoothStep(sinFadeRange.x, sinFadeRange.y, Mathf.Sin(t)*0.5f + 0.5f);
            // realTimeWeight[i] = Mathf.Lerp(sinFadeRange.x, sinFadeRange.y, Mathf.Sin(t)*0.5f + 0.5f);
            realTimeWeight[i] = channelWeight[i];
        //Assign Value Based On Weight
            Shader.SetGlobalFloat(channelDatas[i].channelWeightName, realTimeWeight[i]);
            AudioManager.Instance.SetMixerValue(channelDatas[i].mixerValueName, Mathf.Lerp(-80, 0, realTimeWeight[i]));
        }
    }
}
