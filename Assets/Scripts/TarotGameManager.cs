using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarotGameManager : MonoBehaviour
{
    [SerializeField] private Interact_PlaceCard[] cardPlacers;
    
    private int placerIndex = 0;
    private int flipIndex = 0;
    private List<Card> placedCardList;

    void OnEnable(){
        EventHandler.E_OnPlayerPickUpCard += OnPickCardHandler;
        EventHandler.E_OnPlayerPlaceCard  += OnPlaceCardHandler;
    }
    void OnDisable(){
        EventHandler.E_OnPlayerPickUpCard -= OnPickCardHandler;
        EventHandler.E_OnPlayerPlaceCard  -= OnPlaceCardHandler;
    }
    void Start(){
        placedCardList = new List<Card>();
    }
    void OnPickCardHandler(){
        if(placerIndex<cardPlacers.Length)
            cardPlacers[placerIndex].EnableHitbox();
    }
    void OnPlaceCardHandler(Card card){
        placedCardList.Add(card);
        placerIndex ++;
        if(placerIndex == 3){
            GameController.Instance.ProceedToFlipCard(placedCardList.ToArray());
        }
    }
    public void PrepareNextCard(){
        if(flipIndex>=placedCardList.Count){
            GameController.Instance.ProceedToFinishCard();
        }
        else{
            placedCardList[flipIndex].EnableHitbox();
            flipIndex ++;
        }
    }
    public Card GetPlacedCardByIndex(int index){
        return placedCardList[index];
    }
}
