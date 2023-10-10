using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionWordsController : MonoBehaviour
{
    [SerializeField] private CardWordsSO cardWords_SO;
    void OnEnable(){
        EventHandler.E_OnGetVision += GetVisionHandler;
    }
    void OnDisable(){
        EventHandler.E_OnGetVision -= GetVisionHandler;
    }
    void GetVisionHandler(Card card){
        
    }
}
