using System.Collections;
using System.Collections.Generic;
using SimpleAudioSystem;
using UnityEngine;

public class TarotTriangleMark : MonoBehaviour
{
    [SerializeField] private int cardIndex;
    [SerializeField] private Transform detectCenter;
    [SerializeField] private float offsetToAlpha;
    [SerializeField] private float offsetToPos;
    [SerializeField] private Camera targetCam;
    [SerializeField] private SpriteRenderer[] triangleRenderers;
    [SerializeField] private SpriteRenderer clearTriangles;
[Header("Match")]
    [SerializeField] private float matchRange = 0.2f;
    [SerializeField] private ConditionDetector triangleFormCondition;
[Header("Audio")]
    [SerializeField] private AudioSource hintAudio;
    [SerializeField] private AudioSource foundAudio;
    [SerializeField] private AudioClip[] foundClips;
    public bool m_IsRunning{get; private set;} = false;
    private Color[] initColors;
    private Vector3[] initPos;
    private float ratio;
    private float depth;
    private bool isMatched = false;
    private bool getVision;
    void Start(){
        depth = targetCam.WorldToScreenPoint(detectCenter.position).z;
        ratio = (targetCam.pixelWidth+0f)/(Camera.main.pixelWidth+0f);
        initColors = new Color[triangleRenderers.Length];
        initPos = new Vector3[triangleRenderers.Length];
        for(int i=0; i<initColors.Length; i++){
            initPos[i] = triangleRenderers[i].transform.localPosition;
            initColors[i] = triangleRenderers[i].color;
            triangleRenderers[i].color = Color.clear;
        }
    }
    public void OnStart(bool upsideDown){
        hintAudio.volume = 0;
        hintAudio.Play();

        for(int i=0; i<initColors.Length; i++){
            if(upsideDown) triangleRenderers[i].transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
        if(upsideDown)clearTriangles.transform.localRotation = Quaternion.Euler(0, 0, 180);

        triangleFormCondition.OnConditionMeet(OnConditionMeetHandler);
        triangleFormCondition.OnReset(OnResetHandler);
        triangleFormCondition.OnComplete(OnCompleteHandler);

        m_IsRunning = true;
    }
    public void UpdateTriangle(Vector3 pointerPos){
        Vector3 offset = Vector3.zero;

        if(!getVision){
            pointerPos.z = depth;
            pointerPos.x *= ratio;
            pointerPos.y *= ratio;
            Vector3 pointerWorldPos = targetCam.ScreenToWorldPoint(pointerPos);

            offset = pointerWorldPos-detectCenter.position;
            if(offset.magnitude<=matchRange){
                offset = Vector3.zero;
                if(!isMatched){
                    isMatched = true;
                    for(int i=0; i<triangleRenderers.Length; i++){
                        triangleRenderers[i].enabled = false;
                    }
                    clearTriangles.enabled = true;
                }
            }
            else{
                if(isMatched){
                    isMatched = false;
                    for(int i=0; i<triangleRenderers.Length; i++){
                        triangleRenderers[i].enabled = true;
                    }
                    clearTriangles.enabled = false;
                }
            }
        }

        for(int i=0; i<triangleRenderers.Length; i++){
            triangleRenderers[i].transform.localPosition = initPos[i] + new Vector3(offset.x, offset.z, 0)*offsetToPos*i;
            Color color = initColors[i];
            color.a = Mathf.Lerp(0, initColors[i].a, Mathf.Clamp01(1-offset.magnitude/offsetToAlpha));
            triangleRenderers[i].color = color;
        }

        triangleFormCondition.DetectUpdate(isMatched);
    }
    void OnConditionMeetHandler(){
        AudioManager.Instance.FadeAudio(hintAudio, 1, 0.5f);
        EventHandler.Call_OnFoundVision();
    }
    void OnResetHandler(){
        AudioManager.Instance.FadeAudio(hintAudio, 0, 0.5f);
        EventHandler.Call_OnLostVision();
    }
    void OnCompleteHandler(){
        foundAudio.PlayOneShot(foundClips[Random.Range(0, foundClips.Length)]);
        getVision = true;
        GameController.Instance.LoadVisionTriangle(this);
        EventHandler.Call_OnGetVision(GameController.Instance.GetCardByIndex(cardIndex));
    }
    public void OnFinishVision(){
        getVision = false;
        AudioManager.Instance.FadeAudio(hintAudio, 0, 0.5f);
        EventHandler.Call_OnLostVision();
        StartCoroutine(coroutineHideTriangle());
    }
    IEnumerator coroutineHideTriangle(){
        yield return CommonCoroutine.CoroutineFadeSprite(clearTriangles, 0, 0.2f);
        gameObject.SetActive(false);
    }
}
