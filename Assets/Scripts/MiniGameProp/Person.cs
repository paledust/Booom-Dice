using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
[Header("Person Sprite")]
    [SerializeField] private SpriteRenderer personRenderer;
    [SerializeField] private Sprite[] personSprites;
    [SerializeField] private float floatAmp = 2;
    [SerializeField] private float floatSpeed = 2;
    [SerializeField] private float waveSpeed = 10;
    [SerializeField] private float waveAngle = 10;
    private int spriteIndex = 0;
    void Start(){
        personRenderer.sprite = personSprites[spriteIndex];
    }
    void Update()
    {
        personRenderer.transform.localRotation = Quaternion.Euler(0,0,Mathf.Sin(Time.time*waveSpeed)*waveAngle);   
        personRenderer.transform.localPosition = Vector3.up * Mathf.Sin(Time.time*floatSpeed)*floatAmp;     
    }
    public void SwitchPersonPost(){
        spriteIndex ++;
        spriteIndex = spriteIndex%personSprites.Length;
        personRenderer.sprite = personSprites[spriteIndex];
    }
}
