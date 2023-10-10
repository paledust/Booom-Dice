using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_Dice : BasicPointAndClickInteractable
{
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private float torqueScale = 1;
    [SerializeField] private SFX_Emitter pickDiceEmit;
    public Rigidbody m_rigid{get{return rigid;}}
    public override void OnClick(HandController handController)
    {
        base.OnClick(handController);
        pickDiceEmit.EmitSoundEffect();
        rigid.isKinematic = true;
        handController.Pick_Dice(this);
    }
    public void ApplyThrowForce(Vector3 force){
        rigid.isKinematic = false;
        rigid.AddForce(force, ForceMode.VelocityChange);
        rigid.angularVelocity = Random.insideUnitSphere.normalized*Random.Range(0.9f,1.1f)*torqueScale;
    }
}
