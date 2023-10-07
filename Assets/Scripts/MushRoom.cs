using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MushRoom : MonoBehaviour
{
    [SerializeField] private float growTime = 0.25f;
    [SerializeField] private Vector3 initialSize;
    void OnReset(){
        initialSize = transform.localScale;
    }
    void Awake(){
        initialSize = transform.localScale;
        transform.localScale = Vector3.zero;
    }
    public void MushRoomGrow(Transform trans){
        transform.localPosition = trans.localPosition;
        transform.localRotation = trans.localRotation * Quaternion.Euler(0, 0, Random.Range(-5,5));

        StopAllCoroutines();
        StartCoroutine(coroutineMushroomGrow(growTime, initialSize*Random.Range(0.9f,1.1f)));
    }
    IEnumerator coroutineMushroomGrow(float duration, Vector3 targetScale){
        yield return new WaitForLoop(duration, (t)=>{
            transform.localScale = Vector3.LerpUnclamped(Vector3.zero, targetScale, EasingFunc.Easing.BounceEaseOut(t));
        });
    }
}
