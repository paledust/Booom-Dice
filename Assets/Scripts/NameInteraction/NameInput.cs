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
    [SerializeField] private RectTransform nameGroupRect;
    [SerializeField] private RectTransform pendingGroupRect;
    [SerializeField] private NameText[] nameTexts;

    private Dictionary<Key, NameText> keyToCharacterDict;
    private Stack<NameText> pendingNameTexts;
    private int inputIndex = 0;
    void Start(){
        keyToCharacterDict = new Dictionary<Key, NameText>();
        pendingNameTexts = new Stack<NameText>();
        for(int i=0; i<nameTexts.Length; i++){
            pendingNameTexts.Push(nameTexts[i]);
        }
    }
    void Update()
    {
        foreach(KeyControl key in Keyboard.current.allKeys){
            if(key.wasPressedThisFrame){
                if(key.keyCode.IsTextInputKey() && key.keyCode.ToString().Length==1){
                    if(pendingNameTexts.Count==0) return;

                    char code = key.keyCode.ToString().ToLower()[0];
                    string character = pendingNameTexts.Count==nameTexts.Length?alphabetToCharacter.GetLastNameCharacterFromAlphabet(code):alphabetToCharacter.GetCharacterFromAlphabet(code);

                    var nameText = pendingNameTexts.Pop();
                    nameText.text.text = character;
                    nameText.FadeInText();
                    nameText.transform.SetParent(nameGroupRect);
                    nameText.gameObject.SetActive(true);

                    if(!keyToCharacterDict.ContainsKey(key.keyCode)){
                        keyToCharacterDict[key.keyCode] = nameText;
                    }
                }
            }
            if(key.wasReleasedThisFrame){
                if(key.keyCode.IsTextInputKey() && key.keyCode.ToString().Length==1){
                    if(keyToCharacterDict.ContainsKey(key.keyCode)){
                        var nameText = keyToCharacterDict[key.keyCode];
                        nameText.transform.SetParent(pendingGroupRect);
                        nameText.FadeOutText();
                        pendingNameTexts.Push(nameText);
                        keyToCharacterDict.Remove(key.keyCode);
                    }
                }
            }
        }
    }
}
