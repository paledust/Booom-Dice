using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextSphere : MonoBehaviour
{
    [SerializeField] private SphereState sphereState; 
    [SerializeField] private Vector2 idleSpeed;
    [SerializeField] private TextMeshPro[] texts;
    [SerializeField] private float radius;
    [SerializeField] private float textScale;

    private static readonly Vector3[] textPosVecs = new Vector3[]{Vector3.up, new Vector3(-0.8164f, -0.3333f, -0.4714f), new Vector3(0, -0.3333f, 0.9428f), new Vector3(0.8164f, -0.3333f, -0.4714f)};

    void Update()
    {
        switch(sphereState){
            case SphereState.Idle:
                transform.rotation *= Quaternion.Euler(Mathf.PerlinNoise(0.2f,0.5421f+Time.time)*idleSpeed.x*Time.deltaTime, Mathf.PerlinNoise(0.4212f,Time.time+0.3221f)*idleSpeed.y*Time.deltaTime, 0);
                break;
            case SphereState.Follow:
                break;
        }
        for(int i=0; i<texts.Length; i++){
            texts[i].transform.position = transform.position + transform.rotation * textPosVecs[i]*radius;
            texts[i].transform.localScale = Vector3.one*textScale;
        }
    }
    public void ExpandSphere(string[] words){
        for(int i=0; i<texts.Length; i++){
            texts[i].text = words[i];
        }
        StartCoroutine(coroutineExpandSphere(4, 1));
    }
    IEnumerator coroutineExpandSphere(float targetRadius, float duration){
        yield return new WaitForLoop(duration, (t)=>{
            
        });
    }
}
