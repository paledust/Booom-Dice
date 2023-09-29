using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_PlaceCard : BasicPointAndClickInteractable
{
    [SerializeField] private Transform placeParent;
    public bool m_hasCard{get; private set;}
    public override void OnClick(HandController handController)
    {
        base.OnClick(handController);

        if(handController.HasCard){
            m_hasCard = true;
            handController.PutDown_Card(placeParent);
            DisableHitbox();
        }
    }
}
