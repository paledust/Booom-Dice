using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class NameInput : MonoBehaviour
{
[Header("Input")]
    [SerializeField] private AlphabetToCharacter alphabetToCharacter;
[Header("Text UI")]
    [SerializeField] private NameText[] nameTexts;

    private Dictionary<Key, NameText> keyToCharacterDict;
    [SerializeField, ShowOnly] private List<NameText> pendingNameTexts;
    void Start(){
        keyToCharacterDict = new Dictionary<Key, NameText>();
        pendingNameTexts = new List<NameText>();
        for(int i=0; i<nameTexts.Length; i++){
            pendingNameTexts.Add(nameTexts[i]);
            nameTexts[i].order = i;
        }
    }
    void Update()
    {
        foreach(KeyControl key in Keyboard.current.allKeys){
            if(key.wasPressedThisFrame){
                if(key.keyCode.IsTextInputKey() && key.keyCode.ToString().Length==1){
                    if(keyToCharacterDict.Count==nameTexts.Length) return;
                    var nameText = GetPendingNameText();

                    char code = key.keyCode.ToString().ToLower()[0];
                    string character = nameText.order==0?alphabetToCharacter.GetLastNameCharacterFromAlphabet(code):alphabetToCharacter.GetCharacterFromAlphabet(code);

                    nameText.text.text = character;
                    nameText.FadeInText();

                    if(!keyToCharacterDict.ContainsKey(key.keyCode)){
                        keyToCharacterDict[key.keyCode] = nameText;
                    }
                    pendingNameTexts.Remove(nameText);
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
}
