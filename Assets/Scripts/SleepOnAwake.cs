using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SleepOnAwake : MonoBehaviour
{
    void Awake(){
        GetComponent<Rigidbody>().Sleep();
    }
}
