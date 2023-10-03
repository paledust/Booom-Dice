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
    [SerializeField] private TextMeshProUGUI[] nameTexts;

    private Dictionary<Key, TextMeshProUGUI> keyToCharacterDict;
    private Stack<TextMeshProUGUI> pendingUITexts;
    private int inputIndex = 0;
    void Start(){
        keyToCharacterDict = new Dictionary<Key, TextMeshProUGUI>();
        pendingUITexts = new Stack<TextMeshProUGUI>();
        for(int i=0; i<nameTexts.Length; i++){
            pendingUITexts.Push(nameTexts[i]);
        }
    }
    void Update()
    {
        foreach(KeyControl key in Keyboard.current.allKeys){
            if(key.wasPressedThisFrame){
                if(key.keyCode.IsTextInputKey() && key.keyCode.ToString().Length==1){
                    if(pendingUITexts.Count==0) return;

                    char code = key.keyCode.ToString().ToLower()[0];
                    string character = pendingUITexts.Count==nameTexts.Length?alphabetToCharacter.GetLastNameCharacterFromAlphabet(code):alphabetToCharacter.GetCharacterFromAlphabet(code);

                    var UIText = pendingUITexts.Pop();
                    UIText.text = character;
                    UIText.transform.SetParent(nameGroupRect);
                    UIText.gameObject.SetActive(true);

                    if(!keyToCharacterDict.ContainsKey(key.keyCode)){
                        keyToCharacterDict[key.keyCode] = UIText;
                    }
                }
            }
            if(key.wasReleasedThisFrame){
                if(key.keyCode.IsTextInputKey() && key.keyCode.ToString().Length==1){
                    if(keyToCharacterDict.ContainsKey(key.keyCode)){
                        keyToCharacterDict[key.keyCode].transform.SetParent(pendingGroupRect);
                        keyToCharacterDict[key.keyCode].gameObject.SetActive(false);
                        pendingUITexts.Push(keyToCharacterDict[key.keyCode]);
                        keyToCharacterDict.Remove(key.keyCode);
                    }
                }
            }
        }
    }
}
