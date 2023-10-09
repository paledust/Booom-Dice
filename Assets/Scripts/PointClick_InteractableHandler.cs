using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointClick_InteractableHandler : MonoBehaviour
{
    [SerializeField] private Transform handTipTrans;
    [SerializeField] private float raycastRadius = 0.5f;
    [SerializeField] private HandController handController;

    private BasicPointAndClickInteractable hoveringInteractable;
    private BasicPointAndClickInteractable holdingInteractable;
    private Camera mainCam;

    void Start()=>mainCam = Camera.main;

    public void DetectInteractable()
    {
        Ray ray = mainCam.ScreenPointToRay(mainCam.WorldToScreenPoint(handTipTrans.position));
        if(Physics.SphereCast(ray.origin, raycastRadius, ray.direction, out RaycastHit hit, Mathf.Infinity, Service.interactableLayer)){
            BasicPointAndClickInteractable hit_Interactable = hit.collider.GetComponent<BasicPointAndClickInteractable>();
            if(hit_Interactable!=null){
                if(hoveringInteractable != hit_Interactable) {
                    if(hoveringInteractable!=null) hoveringInteractable.OnExitHover();
                    handController.OnHover();
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
            handController.OnExitHover();
            hoveringInteractable.OnExitHover();
            hoveringInteractable = null;
        }
    }
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(handTipTrans.position, raycastRadius);
    }
}
