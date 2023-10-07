using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    [SerializeField] private Transform renderTrans;
    [SerializeField] private float floatAmp = 2;
    [SerializeField] private float floatSpeed = 2;
    [SerializeField] private float waveSpeed = 10;
    [SerializeField] private float waveAngle = 10;
    void Update()
    {
        renderTrans.localRotation = Quaternion.Euler(0,0,Mathf.Sin(Time.time*waveSpeed)*waveAngle);   
        renderTrans.localPosition = Vector3.up * Mathf.Sin(Time.time*floatSpeed)*floatAmp;     
    }
}
