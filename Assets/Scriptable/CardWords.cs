using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Booom_Dice/CardWords")]
public class CardWords : ScriptableObject
{
    [SerializeField] private List<cardWord> cardWordList;
    public string[] GetCardWords(CardType cardType, bool isUp){
        var cardWord = cardWordList.Find(x=>x.cardType==cardType);
        return isUp?cardWord.upWords:cardWord.downWords;
    }
}

[System.Serializable]
public class cardWord{
    public CardType cardType;
    public string[] upWords;
    public string[] downWords;
}
