using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BasicPointAndClickInteractable : MonoBehaviour
{
    [SerializeField] protected Collider hitbox;
    public virtual void OnHover(PointClick_InteractableHandler pointclick_handler){}
    public virtual void OnExitHover(){}
    public virtual void OnClick(PointClick_InteractableHandler interactableHandler){}
    public virtual void OnRelease(PointClick_InteractableHandler interactableHandler){}
    public void DisableHitbox(){hitbox.enabled = false;}
    public void EnableHitbox(){hitbox.enabled = true;}
}
