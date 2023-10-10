using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Booom_Dice/CardWordsSO")]
public class CardWordsSO : ScriptableObject
{
    [SerializeField] private List<cardWord> cardWordList;
    public string[] GetCardWords(CardType cardType, bool upsideDown){
        var cardWord = cardWordList.Find(x=>x.cardType==cardType);
        return upsideDown?cardWord.downWords:cardWord.upWords;
    }
}

[System.Serializable]
public class cardWord{
    public CardType cardType;
    public string[] upWords;
    public string[] downWords;
}
