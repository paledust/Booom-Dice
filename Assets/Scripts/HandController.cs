using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class HandController : MonoBehaviour
{
    [SerializeField, ShowOnly] private HandState handState = HandState.Default;
[Header("Hand Move")]
    [SerializeField] private Transform handTarget;
    [SerializeField] private float lerpSpeed = 10;
[Header("Hand Interact")]
    [SerializeField] private PointClick_InteractableHandler pointClick_InteractableHandler;
[Header("Pick Cards")]
    [SerializeField] private Transform pickCardTrans;
[Header("Pick Dice")]
    [SerializeField] private Transform pickDiceTrans;
    [SerializeField] private Vector3 ThrowForce;

    private Interact_Dice dice;
    private Card pickedCard;
    private Camera mainCam;
    private float depth;

    public PointClick_InteractableHandler m_PointClick_InteractableHandler{get{return pointClick_InteractableHandler;}}
    public bool HasCard{get{return pickedCard!=null;}}

    void Start(){
        Cursor.lockState = CursorLockMode.Confined;
        mainCam = Camera.main;
        depth = mainCam.WorldToScreenPoint(handTarget.position).z;
    }
    public void Hand_Update(Vector3 pointer){
        UpdateHandPos(pointer);
        pointClick_InteractableHandler.DetectInteractable();
    }
    void UpdateHandPos(Vector3 pointer)
    {
        pointer.z = depth;
        Vector3 targetPosition = mainCam.ScreenToWorldPoint(pointer);
        targetPosition.y = handTarget.position.y;

        handTarget.position = Vector3.Lerp(handTarget.position, targetPosition, Time.deltaTime*lerpSpeed);
    }
#region Hand Interaction
    public void Hand_Interact(bool isPressed){
        pointClick_InteractableHandler.InteractWithInteractable(isPressed, handState);
    }
    public void Pick_Card(Card card){
        pickedCard = card;

        card.transform.parent = pickCardTrans;
        card.transform.localPosition = Vector3.zero;
        card.transform.localRotation = Quaternion.Euler(0,(card.upsideDown?180:0),0);

        handState = HandState.PickCard;

        EventHandler.Call_OnPlayerPickUpCard();
    }
    public void PutDown_Card(Transform cardPlaceTrans){
        pickedCard.transform.parent = cardPlaceTrans.transform;
        pickedCard.transform.localPosition = Vector3.zero;
        pickedCard.transform.localRotation = Quaternion.Euler(0, (pickedCard.upsideDown?180:0), 180);

        pickedCard = null;
        
        handState = HandState.Default;

        EventHandler.Call_OnPlayerPlaceCard();
    }
    public void Pick_Dice(Interact_Dice dice){
        this.dice = dice;

        dice.m_rigid.transform.parent = pickDiceTrans;
        dice.m_rigid.transform.localPosition = Vector3.zero;
        dice.m_rigid.transform.localRotation = Quaternion.Euler(Random.Range(0, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

        handState = HandState.PickDice;
    }
    public void Throw_Dice(){
        dice.m_rigid.transform.parent = null;
        dice.AddThrowForce(ThrowForce);

        this.dice = null;

        handState = HandState.Default;
    }
#endregion
    void OnDrawGizmosSelected(){
        DebugExtension.DrawArrow(pickDiceTrans.position,ThrowForce, Color.green);
    }
}
