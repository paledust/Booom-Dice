using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Booom_Dice/CardToMiniGame")]
public class CardToMiniGame : ScriptableObject
{
    [SerializeField] private List<MiniGameData> miniGames;
    public GameObject GetMiniGamePrefabFromCardType(CardType cardType){
        return miniGames.Find(x=>x.cardType==cardType).miniGamePrefab;
    }
}

[System.Serializable]
public class MiniGameData{
    public CardType cardType;
    public GameObject miniGamePrefab;
}