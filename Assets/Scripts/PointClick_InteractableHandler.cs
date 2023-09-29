using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointClick_InteractableHandler : MonoBehaviour
{
    [SerializeField] private Transform handTipTrans;
    [SerializeField] private HandController handController;

    private BasicPointAndClickInteractable hoveringInteractable;
    private BasicPointAndClickInteractable holdingInteractable;
    private Camera mainCam;

    void Start()=>mainCam = Camera.main;

    public void DetectInteractable()
    {
        Ray ray = mainCam.ScreenPointToRay(mainCam.WorldToScreenPoint(handTipTrans.position));
        Debug.DrawRay(ray.origin, ray.direction, Color.green);
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Service.interactableLayer)){
            BasicPointAndClickInteractable hit_Interactable = hit.collider.GetComponent<BasicPointAndClickInteractable>();
            if(hit_Interactable!=null){
                if(hoveringInteractable != hit_Interactable) {
                    if(hoveringInteractable!=null) hoveringInteractable.OnExitHover();
                    hoveringInteractable = hit_Interactable;
                    hoveringInteractable.OnHover(handController);
                }
            }
            else{
                ClearCurrentInteractable();
            }
        }
        else{
            ClearCurrentInteractable();
        }
    }
    public void InteractWithInteractable(bool isPressed, HandState handState){
        if(isPressed){

            switch(handState){
                case HandState.Default:
                    if(hoveringInteractable==null) return;
                    if(holdingInteractable!=null) return;
                    hoveringInteractable.OnClick(handController);
                    break;
                case HandState.PickCard:
                    if(hoveringInteractable==null) return;
                    if(holdingInteractable!=null) return;
                    if(hoveringInteractable.GetType()==typeof(Interact_PlaceCard)){
                        hoveringInteractable.OnClick(handController);
                    }
                    break;
                case HandState.PickDice:
                    Debug.Log(">>>");
                    handController.Throw_Dice();
                    break;
            }
        }
        else{
            if(holdingInteractable!=null){
                holdingInteractable.OnRelease(handController);
                holdingInteractable = null;
            }
        }
    }

    void ClearCurrentInteractable()
    {
        if(hoveringInteractable != null){
            hoveringInteractable.OnExitHover();
            hoveringInteractable = null;
        }
    }
}
