using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_PlaceCard : BasicPointAndClickInteractable
{
    [SerializeField] private Transform placeParent;
    public override void OnClick(HandController handController)
    {
        base.OnClick(handController);

        if(handController.HasCard){
            handController.PutDown_Card(placeParent);
            DisableHitbox();
        }
    }
}
