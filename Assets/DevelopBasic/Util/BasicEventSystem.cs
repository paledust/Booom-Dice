using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A basic C# Event System
public static class EventHandler
{
    public static event Action E_BeforeUnloadScene;
    public static void Call_BeforeUnloadScene(){E_BeforeUnloadScene?.Invoke();}
    public static event Action E_AfterLoadScene;
    public static void Call_AfterLoadScene(){E_AfterLoadScene?.Invoke();}
    
    public static event Action E_OnPlayerPickUpCard;
    public static void Call_OnPlayerPickUpCard()=>E_OnPlayerPickUpCard?.Invoke();
    public static event Action<Card> E_OnPlayerPlaceCard;
    public static void Call_OnPlayerPlaceCard(Card cardType)=>E_OnPlayerPlaceCard?.Invoke(cardType);
    public static event Action<Card> E_OnFlipCard;
    public static void Call_OnFlipCard(Card card)=>E_OnFlipCard?.Invoke(card);

    public static event Action E_OnPickUpDice;
    public static void Call_OnPickUpDice()=>E_OnPickUpDice?.Invoke();
    public static event Action E_OnDropDice;
    public static void Call_OnDropDice()=>E_OnDropDice?.Invoke();

    public static event Action<LookableType> E_OnSpotLookable;
    public static void Call_OnSpotLookable(LookableType lookableType)=>E_OnSpotLookable?.Invoke(lookableType);

    public static event Action<int> E_OnFoundVision;
    public static void Call_OnFoundVision(int visionIndex)=>E_OnFoundVision?.Invoke(visionIndex);
    public static event Action<int> E_OnLostVision;
    public static void Call_OnLostVision(int visionIndex)=>E_OnLostVision?.Invoke(visionIndex);
    public static event Action<Card> E_OnGetVision;
    public static void Call_OnGetVision(Card card)=>E_OnGetVision?.Invoke(card);
}