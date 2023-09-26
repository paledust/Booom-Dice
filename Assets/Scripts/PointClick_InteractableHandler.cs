using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointClick_InteractableHandler : MonoBehaviour
{
    [SerializeField] private Transform handTipTrans;

    private BasicPointAndClickInteractable hoveringInteractable;
    private BasicPointAndClickInteractable holdingInteractable;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }
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
                    hoveringInteractable.OnHover(this);
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
    public void InteractWithInteractable(bool isPressed){
        if(isPressed){
            if(hoveringInteractable==null) return;
            if(holdingInteractable!=null) return;

            hoveringInteractable.OnClick(this);
        }
        else{
            if(holdingInteractable!=null){
                holdingInteractable.OnRelease(this);
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
