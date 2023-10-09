using System.Collections;
using System.Collections.Generic;
using SimpleAudioSystem;
using UnityEngine;

public class Interact_Deck : BasicPointAndClickInteractable
{
[Header("Card Layout")]
    [SerializeField] private Card[] cards;
    [SerializeField] private float cardHeight;
    [SerializeField] private float cardIntersection;
    [SerializeField] private AudioSource sfx_audio;
    [SerializeField] private AudioClip[] sfX_pickCardClip;

    private const int maxCount = 3;
    private int cardIndex;

    void Start(){
        Service.Shuffle<Card>(ref cards);
        cardIndex = cards.Length-1;
        for(int i=0; i<cards.Length; i++){
            bool upsideDown = Random.value>0.5f;

            Vector3 pos = transform.position;
            pos.y = cardHeight + cardIntersection*i;
            cards[i].upsideDown = upsideDown;
            cards[i].transform.localRotation = Quaternion.Euler(0,(upsideDown?180:0),180);
            cards[i].transform.position = pos;
        }
    }
    public override void OnClick(HandController handController)
    {
        base.OnClick(handController);
        AudioManager.Instance.PlaySoundEffect(sfx_audio, sfX_pickCardClip[Random.Range(0, sfX_pickCardClip.Length)], 1);
        handController.Pick_Card(cards[cardIndex]);

        cardIndex --;
        if(cards.Length - cardIndex > maxCount){
            DisableHitbox();
        }
    }
}
