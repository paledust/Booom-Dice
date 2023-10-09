using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private Animation cloudAnimation;
    [SerializeField] private float moveUpSpeed = 0.02f;
    [SerializeField] private float waveSpeed = 2;
    [SerializeField] private float waveAmp = 0.01f;
    private Vector3 initPos;
    private float startTime;
    public void ActiveCloud(Vector3 pos){
        transform.position = initPos = pos;
        startTime = Time.time;
        gameObject.SetActive(true);
    }
    void Update(){
        float t = (Time.time-startTime);
        transform.position = initPos + Vector3.forward * moveUpSpeed * t + Vector3.right * Mathf.Sin(t*Mathf.PI*waveSpeed)*waveAmp;
        if(Mathf.Abs(transform.position.z - initPos.z)>3){
            gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter(Collider other){
        Person person = other.GetComponent<Person>();
        if(person!=null){
            person.SwitchPersonPost();
            cloudAnimation.Play();
        }
    }
}
