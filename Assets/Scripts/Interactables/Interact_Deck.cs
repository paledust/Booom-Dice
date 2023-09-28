using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_Deck : BasicPointAndClickInteractable
{
[Header("Card Layout")]
    [SerializeField] private Transform[] cards;
    [SerializeField] private float cardHeight;
    [SerializeField] private float cardIntersection;

    private int cardIndex;

    void Start(){
        Service.Shuffle<Transform>(ref cards);
        cardIndex = cards.Length-1;
        for(int i=0; i<cards.Length; i++){
            Vector3 pos = transform.position;
            pos.y = cardHeight + cardIntersection*i;
            cards[i].localRotation = Quaternion.Euler(0,(Random.value>0.5f?180:0),180);
            cards[i].position = pos;
        }
    }
    public override void OnClick(HandController handController)
    {
        base.OnClick(handController);
        handController.Pick_Card(cards[cardIndex]);
        cardIndex --;
    }
}
