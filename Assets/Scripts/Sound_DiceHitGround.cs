using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_DiceHitGround : MonoBehaviour
{
    [SerializeField] private SFX_Emitter collisionSound;
    void OnCollisionEnter(Collision other){
        collisionSound.EmitSoundEffect();
    }
}
