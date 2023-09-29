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
    public static event Action E_OnPlayerPlaceCard;
    public static void Call_OnPlayerPlaceCard()=>E_OnPlayerPlaceCard?.Invoke();
}