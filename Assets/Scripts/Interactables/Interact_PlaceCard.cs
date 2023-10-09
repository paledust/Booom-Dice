using System.Collections;
using System.Collections.Generic;
using SimpleAudioSystem;
using UnityEngine;

public class Interact_PlaceCard : BasicPointAndClickInteractable
{
    [SerializeField] private Transform placeParent;
    [SerializeField] private AudioSource sfx_audio;
    [SerializeField] private AudioClip putcardClip;
    public bool m_hasCard{get; private set;}
    public override void OnClick(HandController handController)
    {
        base.OnClick(handController);

        if(handController.HasCard){
            m_hasCard = true;
            handController.PutDown_Card(placeParent);
            AudioManager.Instance.PlaySoundEffect(sfx_audio, putcardClip, 0.5f);
            DisableHitbox();
        }
    }
}
