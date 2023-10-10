using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dice_Dice : MonoBehaviour
{
    [SerializeField] private Light diceLight;
    void OnEnable(){
        EventHandler.E_OnGetVision += GetVisionHandler;
    }
    void OnDisable(){
        EventHandler.E_OnGetVision -= GetVisionHandler;
    }
    void GetVisionHandler(Card card){
        diceLight.DOIntensity(3, 0.5f);
    }
    public void OnBeenThrown(){
        diceLight.DOIntensity(1, 0.25f);
    }
}
