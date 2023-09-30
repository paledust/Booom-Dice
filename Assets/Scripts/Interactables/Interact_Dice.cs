using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_Dice : BasicPointAndClickInteractable
{
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private float torqueScale = 1;
    public Rigidbody m_rigid{get{return rigid;}}

    public override void OnClick(HandController handController)
    {
        base.OnClick(handController);
        rigid.isKinematic = true;
        handController.Pick_Dice(this);
    }
    public void AddThrowForce(Vector3 force){
        rigid.isKinematic = false;
        rigid.AddForce(force, ForceMode.VelocityChange);
        rigid.angularVelocity = Random.insideUnitSphere.normalized*Random.Range(0.9f,1.1f)*torqueScale;
    }
}
