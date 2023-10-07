using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushRoomDetect : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        var tree = other.GetComponent<MovingTree>();
        if(tree!=null){
            Debug.Log("Grow");
            if(!tree.m_growed) tree.GrowMushRoom();
        }
    }
}
