using System.Collections;
using System.Collections.Generic;
using SimpleAudioSystem;
using UnityEngine;

[System.Serializable]
public class SFX_Emitter
{
    public AudioSource m_audio;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private float clipIntersection = 1;
    [SerializeField] private float volumeScale = 1;
    [SerializeField] private bool testTimeDuringEmit = false;
    private float audioTimer = 0;
    private int clipIndex = 0;

    public void ShuffleClip(){
        Service.Shuffle(ref clips);
    }
    public void KeepEmitSound(){
        if(audioTimer+clipIntersection<Time.time){
            EmitSoundEffect();
        }
    }
    public void KeepEmitSound(bool condition){
        if(audioTimer+clipIntersection<Time.time && condition){
            EmitSoundEffect();
        }
    }
    public void EmitSoundEffect(){
        if(testTimeDuringEmit && audioTimer+clipIntersection>Time.time) return;
        
        AudioManager.Instance.PlaySoundEffect(m_audio, clips[clipIndex], volumeScale);
        audioTimer = Time.time;

        clipIndex++;
        if(clipIndex>=clips.Length){
            clipIndex = 0;
            Service.Shuffle(ref clips);
        }
    }
}
