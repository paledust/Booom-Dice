using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_Deck : BasicPointAndClickInteractable
{
[Header("Card Layout")]
    [SerializeField] private Card[] cards;
    [SerializeField] private float cardHeight;
    [SerializeField] private float cardIntersection;

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
        handController.Pick_Card(cards[cardIndex]);

        cardIndex --;
        if(cards.Length - cardIndex > maxCount){
            DisableHitbox();
        }
    }
}
