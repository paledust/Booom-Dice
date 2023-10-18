using System.Collections;
using System.Collections.Generic;
using SimpleAudioSystem;
using UnityEngine;

public class Interact_Candle : BasicPointAndClickInteractable
{
    [SerializeField] private ParticleSystem candleFlame;
    void OnEnable(){
        EventHandler.E_AfterLoadScene += DisableHitbox;
        EventHandler.E_AfterSceneTransist += EnableHitbox;
    }
    void OnDisable(){
        EventHandler.E_AfterLoadScene -= DisableHitbox;
        EventHandler.E_AfterSceneTransist -= EnableHitbox;
    }
    public override void OnClick(HandController handController)
    {
        base.OnClick(handController);

        AudioManager.Instance.PlayOverallSFX("sfx_candleoff", 1);
        candleFlame.Stop();
        handController.PutOutCandle(this);

        StartCoroutine(coroutinePutOutCandle());
    }
    IEnumerator coroutinePutOutCandle(){
        yield return new WaitForSeconds(0.15f);
        GameManager.Instance.RestartLevel(0.1f, 1f);
    }
}
