using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

[System.Serializable]
public class HandController : MonoBehaviour
{
    [SerializeField, ShowOnly] private HandState handState = HandState.Default;
    [SerializeField] private GameController gameController;
[Header("Hand IK")]
    [SerializeField] private IK_Control handIK;
    [SerializeField] private Transform handRootTrans;
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
[Header("Flip Card")]
    [SerializeField] private Transform cardParent;
    [SerializeField] private float diffToAngle = 10;
    [SerializeField] private float flipThreashold = 1;
    [SerializeField] private Transform flipCardTrans;
    [SerializeField] private Transform rotateTrans;
    [SerializeField] private Vector3 rotateOffset;
    [SerializeField] private Vector3 cardPosOffset;
    [SerializeField] private Vector3 cardRotEuler;
[Space(20)]
[Header("Animation")]
    [SerializeField] private Animator hand_animator;

    private float depth;
    private Interact_Dice pickedDice;
    private Card pickedCard;
    private Card flipingCard;
    private Camera mainCam;
    private Vector3 pointerPos;

    private float flipValue;
    private Vector3 flipPointerRefPos;
    private Vector3 initHandLocalPos;
    private Quaternion initHandLocalRot;

    private Vector3 initCardPos;

    public bool HasCard{get{return pickedCard!=null;}}

    void Start(){
        Cursor.lockState = CursorLockMode.Confined;
        mainCam = Camera.main;
        depth = mainCam.WorldToScreenPoint(handTarget.position).z;

        initHandLocalPos = handRootTrans.localPosition;
        initHandLocalRot = handRootTrans.localRotation;
    }
    public void Hand_Update(Vector3 pointer){
        pointerPos = pointer;
        pointerPos.z = depth;
        Vector3 targetPosition = mainCam.ScreenToWorldPoint(pointerPos);
        targetPosition.y = handTarget.position.y;

        switch(handState){
            case HandState.FlipCard:
                float diff = flipPointerRefPos.x - targetPosition.x;
                diff = Mathf.Min(0, diff);
                flipValue = Mathf.Lerp(flipValue, diff, Time.deltaTime*10);
                Quaternion rot = Quaternion.Euler(0,0,flipValue*diffToAngle);
                handRootTrans.rotation = rot * Quaternion.Euler(cardRotEuler);
                handRootTrans.position = rotateTrans.position + rot * (cardPosOffset-rotateOffset);

                gameController.UpdateChannelMask(Mathf.Abs(flipValue/flipThreashold));
                if(Mathf.Abs(flipValue)>flipThreashold){
                    EndFlipCard();
                }
                break;
            default:
                handTarget.position = Vector3.Lerp(handTarget.position, targetPosition, Time.deltaTime*lerpSpeed);
                pointClick_InteractableHandler.DetectInteractable();
                break;
        }
    }
    public void OnHover(){
        hand_animator.SetBool("IsHovering", true);
    }
    public void OnExitHover(){
        hand_animator.SetBool("IsHovering", false);
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

        hand_animator.SetTrigger("PickCard");

        EventHandler.Call_OnPlayerPickUpCard();
    }
    public void PutDown_Card(Transform cardPlaceTrans){
        Card card = pickedCard;

        pickedCard.transform.parent = cardPlaceTrans.parent;
        pickedCard.transform.localPosition = cardPlaceTrans.localPosition;   
        pickedCard.transform.localRotation = Quaternion.Euler(0, pickedCard.upsideDown?180:0, 180);

        pickedCard = null;
        
        handState = HandState.Default;

        hand_animator.SetTrigger("DropCard");

        EventHandler.Call_OnPlayerPlaceCard(card);
    }
    public void Pick_Dice(Interact_Dice dice){
        this.pickedDice = dice;

        dice.m_rigid.transform.parent = pickDiceTrans;
        dice.m_rigid.transform.localPosition = Vector3.zero;
        dice.m_rigid.transform.localRotation = Quaternion.Euler(Random.Range(0, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

        hand_animator.SetTrigger("PickDice");

        handState = HandState.PickDice;
    }
    public void Throw_Dice(){
        pickedDice.m_rigid.transform.parent = null;
        pickedDice.ApplyThrowForce(ThrowForce);

        this.pickedDice = null;

        hand_animator.SetTrigger("DropDice");

        handState = HandState.Default;
    }
    public void StartFlipCard(Card card){
        flipValue = 0;
        pointerPos.z = depth;
        initCardPos = card.transform.position;
        flipPointerRefPos = mainCam.ScreenToWorldPoint(pointerPos);
        flipPointerRefPos.y = handTarget.position.y;

        rotateTrans.position = card.transform.position + rotateOffset;

        gameController.OnStartFlipCard(card);
        StartCoroutine(coroutineStartFlipCard(card));
        flipingCard = card;
        handState = HandState.FlipCard;
    }
    public void EndFlipCard(){
        StartCoroutine(coroutineEndFlipCard(flipingCard));
        flipingCard = null;
        handState = HandState.Default;
    }
#endregion
    IEnumerator coroutineStartFlipCard(Card card){
        yield return CommonCoroutine.CoroutineSetTrans(handRootTrans, card.transform.position+cardPosOffset, Quaternion.Euler(cardRotEuler), false, 0.25f);
        card.transform.parent = flipCardTrans;
        card.transform.localPosition = Vector3.zero;
        card.transform.localRotation = card.upsideDown?Quaternion.Euler(0, 180, 0):Quaternion.identity;
    }
    IEnumerator coroutineEndFlipCard(Card card){
        StartCoroutine(coroutineLaydownCard(card));
        yield return CommonCoroutine.CoroutineSetTrans(handRootTrans, handRootTrans.position + Vector3.right*0.5f, handRootTrans.rotation, false, 0.5f);
        yield return new WaitForSeconds(0.15f);
        yield return CommonCoroutine.CoroutineSetTrans(handRootTrans, initHandLocalPos, initHandLocalRot, true, 0.5f);
    }
    IEnumerator coroutineLaydownCard(Card card){
        card.transform.parent = cardParent;
        Vector3 euler = card.transform.eulerAngles;
        euler.x = 0;
        euler.y += Random.Range(-10f, 10f);
        euler.z = 360;

        yield return CommonCoroutine.CoroutineSetTrans(card.transform, initCardPos, Quaternion.Euler(euler), false, .35f, EasingFunc.Easing.FunctionType.QuadEaseIn);
        EventHandler.Call_OnFlipCard(card);
    }
    void OnDrawGizmosSelected(){
        DebugExtension.DrawArrow(pickDiceTrans.position,ThrowForce, Color.green);
    }
}
