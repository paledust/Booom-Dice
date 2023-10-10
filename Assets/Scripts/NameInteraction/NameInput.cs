using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using TMPro;

public class NameInput : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI set_text;
[Header("Input")]
    [SerializeField] private AlphabetToCharacter alphabetToCharacter;
[Header("Text UI")]
    [SerializeField] private NameText[] nameTexts;
[Header("Shadow")]
    [SerializeField] private FaceShadow shadow;
[Header("Detect")]
    [SerializeField] private ConditionDetector NameComplete;
[Header("Ending")]
    [SerializeField] private float endTextFadeDuration = 3;
[Header("Audio")]
    [SerializeField] private SFX_Emitter inputEmiter;
    [SerializeField] private AudioClip completeClip;

    private bool complete = false;
    private Dictionary<Key, NameText> keyToCharacterDict;
    private List<NameText> pendingNameTexts;

    void OnApplicationFocus(bool focus){
        if(!complete && !focus){
            keyToCharacterDict.Clear();
            pendingNameTexts.Clear();
            for(int i=0; i<nameTexts.Length; i++){
                pendingNameTexts.Add(nameTexts[i]);
                nameTexts[i].order = i;
                nameTexts[i].text.text  = string.Empty;
            }
        }
    }
    void Start(){
        NameComplete.OnComplete(OnCompleteNameInput);
        NameComplete.OnConditionMeet(OnConditionMeet);
        NameComplete.OnReset(OnReset);

        keyToCharacterDict = new Dictionary<Key, NameText>();
        pendingNameTexts = new List<NameText>();
        for(int i=0; i<nameTexts.Length; i++){
            pendingNameTexts.Add(nameTexts[i]);
            nameTexts[i].order = i;
        }
    }
    void Update(){
        foreach(KeyControl key in Keyboard.current.allKeys){
            if(key.wasPressedThisFrame){
                if(key.keyCode.IsTextInputKey() && key.keyCode.ToString().Length==1){
                    if(!keyToCharacterDict.ContainsKey(key.keyCode)){
                        if(keyToCharacterDict.Count==nameTexts.Length) return;
                        var nameText = GetPendingNameText();

                        char code = key.keyCode.ToString().ToLower()[0];
                        string character = nameText.order==0?alphabetToCharacter.GetLastNameCharacterFromAlphabet(code):alphabetToCharacter.GetCharacterFromAlphabet(code);

                        nameText.text.text = character;
                        nameText.FadeInText();
                        keyToCharacterDict[key.keyCode] = nameText;
                        pendingNameTexts.Remove(nameText);
                        inputEmiter.EmitSoundEffect();
                    }
                }
            }
            if(key.wasReleasedThisFrame){
                if(key.keyCode.IsTextInputKey() && key.keyCode.ToString().Length==1){
                    if(keyToCharacterDict.ContainsKey(key.keyCode)){
                        var nameText = keyToCharacterDict[key.keyCode];
                        nameText.FadeOutText();
                        keyToCharacterDict.Remove(key.keyCode);
                        pendingNameTexts.Add(nameText);
                    }
                }
            }
        }

        float ratio = NameComplete.DetectUpdate(pendingNameTexts.Count==0);
        set_text.color = new Color(1,1,1, 1-ratio);
    }
    NameText GetPendingNameText(){
        NameText nameText = null;
        int order = 2;
        for(int i=0; i<pendingNameTexts.Count; i++){
            if(pendingNameTexts[i].order<=order){
                order = pendingNameTexts[i].order;
                nameText = pendingNameTexts[i];
            }
        }
        return nameText;
    }
    void OnCompleteNameInput(){
        shadow.BrightFace();
        complete = true;
        this.enabled = false;

        inputEmiter.m_audio.PlayOneShot(completeClip);

        GameManager.Instance.StoredName = string.Empty;
        for(int i=0; i<nameTexts.Length; i++){
            if(nameTexts[i].text.text==null || nameTexts[i].text.text==string.Empty){
                GameManager.Instance.StoredName += " ";
            }
            else{
                GameManager.Instance.StoredName += nameTexts[i].text.text;
            }
        }
        StartCoroutine(EndSequence());
    }
    void OnConditionMeet(){
        shadow.DissolveInShadow();
    }
    void OnReset(){
        shadow.DissolveOutShadow();
    }
    IEnumerator EndSequence(){
        yield return new WaitForSeconds(0.5f);
        for(int i=0; i<nameTexts.Length; i++){
            nameTexts[i].FadeOutText(endTextFadeDuration);
            yield return new WaitForSeconds(Random.Range(0.1f,0.2f));
        }
        yield return new WaitForSeconds(endTextFadeDuration);
        GameManager.Instance.SwitchingScene("Main",false);
    }
}
