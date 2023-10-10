using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionWordsController : MonoBehaviour
{
    [SerializeField] private CardWordsSO cardWords_SO;
    [SerializeField] private TextSphere textSphere;
    [SerializeField] private Dice_Dice dice;
    private bool inVision = false;
    void OnEnable(){
        EventHandler.E_OnGetVision  += GetVisionHandler;
        EventHandler.E_OnPickUpDice += PickUpDiceHandler;
        EventHandler.E_OnDropDice   += DropDice;
    }
    void OnDisable(){
        EventHandler.E_OnGetVision  -= GetVisionHandler;
        EventHandler.E_OnPickUpDice -= PickUpDiceHandler;
        EventHandler.E_OnDropDice   -= DropDice;
    }
    void GetVisionHandler(Card card){
        inVision = true;
        textSphere.ExpandSphere(cardWords_SO.GetCardWords(card.m_cardType, card.upsideDown));
    }
    void PickUpDiceHandler(){
        if(inVision) textSphere.FollowDice(dice.transform);
    }
    void DropDice(){
        if(inVision){
            dice.OnBeenThrown();
            textSphere.DetectingWords();
        }
    }
}
