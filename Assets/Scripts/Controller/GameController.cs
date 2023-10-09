using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class MiniGameRT_Group{
    public Transform miniGameRoot;
    public Camera miniGameRT_Cam;
    public SpriteRenderer miniGameMaskRenderer;
}
public class GameController : MonoBehaviour
{
    [SerializeField] private ParticleSystem hintParticle;
[Header("Hand Control")]
    [SerializeField] private HandController _hand;
[Header("Tarot Game")]
    [SerializeField] private TarotGameManager tarotGame;
[Space(20), Header("Mini Game")]
    [SerializeField] private CardToMiniGame cardToMiniGame;
    [SerializeField] private GameObject miniGameMaskRT_Group;
    [SerializeField] private MiniGameRT_Group[] miniGamesRT_Group;
    [SerializeField] private List<BasicMiniGameController> miniGames;

    private int flipCardIndex = 0;
    private Vector3 pointerPos;

    private static readonly Vector3 miniGamePlacePos = new Vector3(0,0,1);
    private static readonly Vector3 miniGamePlaceRot = new Vector3(-90,0,0);

    void OnEnable(){
        EventHandler.E_OnFlipCard += OnFlipCardHandler;
    }
    void OnDisable(){
        EventHandler.E_OnFlipCard -= OnFlipCardHandler;
    }
    void Update(){
        _hand.Hand_Update(pointerPos);
        foreach(BasicMiniGameController miniGame in miniGames){
            if(miniGame!=null && miniGame.m_updating){
                miniGame.UpdateMiniGame(pointerPos);
            }
        }
    }
    void OnPointer(InputValue value){
        pointerPos = value.Get<Vector2>();
    }
    void OnInteract(InputValue value){
        _hand.Hand_Interact(value.isPressed);
    }
    public void ProceedToFlipCard(Card[] cards){
        hintParticle.Play();
        for(int i=0; i<cards.Length; i++){
            var miniGame = GameObject.Instantiate(cardToMiniGame.GetMiniGamePrefabFromCardType(cards[i].m_cardType)).GetComponent<BasicMiniGameController>();
            miniGame.SetUp(miniGamesRT_Group[i].miniGameRT_Cam);
            miniGame.gameObject.SetActive(false);
            miniGame.transform.parent = miniGamesRT_Group[i].miniGameRoot;
            miniGame.transform.localPosition = miniGamePlacePos;
            miniGame.transform.localEulerAngles = miniGamePlaceRot;
            miniGames.Add(miniGame);
        }
        miniGameMaskRT_Group.SetActive(true);
        tarotGame.PrepareNextCard();
    }
    public void ProceedToFinishCard(){
        hintParticle.Play();
    }
    public void OnFlipCardHandler(Card card){
        tarotGame.PrepareNextCard();

        miniGames[flipCardIndex].gameObject.SetActive(true);
        miniGames[flipCardIndex].StartMiniGame();

        miniGamesRT_Group[flipCardIndex].miniGameRoot.gameObject.SetActive(true);
        StartCoroutine(CommonCoroutine.CoroutineDissolveSprite(miniGamesRT_Group[flipCardIndex].miniGameMaskRenderer, 0, 0.35f, 2f));
        flipCardIndex ++;
    }
}
