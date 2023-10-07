using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarotGameManager : MonoBehaviour
{
    [SerializeField] private Interact_PlaceCard[] cardPlacers;

    private int placerIndex = 0;

    void OnEnable(){
        EventHandler.E_OnPlayerPickUpCard += OnPickCardHandler;
        EventHandler.E_OnPlayerPlaceCard  += OnPlaceCardHandler;
    }
    void OnDisable(){
        EventHandler.E_OnPlayerPickUpCard -= OnPickCardHandler;
        EventHandler.E_OnPlayerPlaceCard  -= OnPlaceCardHandler;
    }
    void OnPickCardHandler(){
        if(placerIndex<cardPlacers.Length){
            cardPlacers[placerIndex].EnableHitbox();
            placerIndex++;
        }
    }
    void OnPlaceCardHandler(){
    }
}
