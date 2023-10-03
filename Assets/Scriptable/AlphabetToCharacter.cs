using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Booom_Dice/AlphabetCharacter")]
public class AlphabetToCharacter : ScriptableObject
{
    [SerializeField] private List<CharacterGroup> characterGroups;
    public string GetCharacterFromAlphabet(char alphabet){
        var group = characterGroups.Find(x=>x.alphabet==alphabet);
        if(group==null) return null;

        List<string> characters = new List<string>();
        characters.AddRange(group.otherCharacters);
        return characters[Random.Range(0, characters.Count)];
    }
    public string GetLastNameCharacterFromAlphabet(char alphabet){
        var group = characterGroups.Find(x=>x.alphabet==alphabet);
        if(group==null) return null;

        string[] characters = group.lastNameCharacters;
        return characters[Random.Range(0, characters.Length)];
    }
}

[System.Serializable]
public class CharacterGroup{
    public char alphabet;
    public string[] lastNameCharacters;
    public string[] otherCharacters;
}
