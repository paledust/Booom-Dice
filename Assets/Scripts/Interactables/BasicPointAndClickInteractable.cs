using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BasicPointAndClickInteractable : MonoBehaviour
{
    [SerializeField] protected Collider hitbox;
    [SerializeField] protected HoverType hoverType;
    public virtual void OnHover(HandController handController){}
    public virtual void OnExitHover(){}
    public virtual void OnClick(HandController handController){}
    public virtual void OnRelease(HandController handController){}
    public void DisableHitbox(){hitbox.enabled = false;}
    public void EnableHitbox(){hitbox.enabled = true;}
}
