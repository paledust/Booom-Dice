using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Card : BasicPointAndClickInteractable
{
    public bool upsideDown = false;
    public CardType m_cardType;
    public override void OnClick(HandController handController)
    {
        base.OnClick(handController);
        handController.StartFlipCard(this);
        
        DisableHitbox();
    }
}
