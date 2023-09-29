using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarotDeskController : MonoBehaviour
{
    [SerializeField] private Interact_PlaceCard[] cardPlacers;
    void OnEnable(){
        EventHandler.E_OnPlayerPickUpCard += OnPickCardHandler;
        EventHandler.E_OnPlayerPlaceCard  += OnPlaceCardHandler;
    }
    void OnDisable(){
        EventHandler.E_OnPlayerPickUpCard -= OnPickCardHandler;
        EventHandler.E_OnPlayerPlaceCard  -= OnPlaceCardHandler;
    }
    void OnPickCardHandler(){
        for(int i=0; i<cardPlacers.Length; i++){
            if(!cardPlacers[i].m_hasCard) cardPlacers[i].EnableHitbox();
        }
    }
    void OnPlaceCardHandler(){
        for(int i=0; i<cardPlacers.Length; i++){
            cardPlacers[i].DisableHitbox();
        }
    }
}
