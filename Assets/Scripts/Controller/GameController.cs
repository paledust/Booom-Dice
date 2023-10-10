using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

[System.Serializable]
public class MiniGameRT_Group{
    public Transform miniGameRoot;
    public Camera miniGameRT_Cam;
    public TarotTriangleMark tarotTriangleMark;
    public SpriteRenderer miniGameMaskRenderer;
}
public class GameController : Singleton<GameController>
{
    [SerializeField] private ParticleSystem hintParticle;
[Header("Hand Control")]
    [SerializeField] private HandController _hand;
[Header("Tarot Game")]
    [SerializeField] private TarotGameManager tarotGame;
[Space(20), Header("Mini Game")]
    [SerializeField] private MiniGameChannelController miniGameControl;
    [SerializeField] private CardToMiniGame cardToMiniGame;
    [SerializeField] private GameObject miniGameMaskRT_Group;
    [SerializeField] private MiniGameRT_Group[] miniGamesRT_Group;
    [SerializeField] private List<BasicMiniGameController> miniGames;
[Header("Audio")]
    [SerializeField] private AudioMixerGroup[] mixerGroups;

    private int flipCardIndex = 0;
    private Vector3 pointerPos;

    public const string DissolveRadiusName = "_DissolveRadius";
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
        for(int i=0; i<miniGamesRT_Group.Length; i++){
            if(miniGamesRT_Group[i].tarotTriangleMark.m_IsRunning){
                miniGamesRT_Group[i].tarotTriangleMark.UpdateTriangle(pointerPos);
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
            miniGame.SetUp(miniGamesRT_Group[i].miniGameRT_Cam, mixerGroups[i]);
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

        miniGameControl.FadeInChannel(flipCardIndex);

        miniGamesRT_Group[flipCardIndex].tarotTriangleMark.OnStart(card.upsideDown);
        StartCoroutine(CommonCoroutine.CoroutineDissolveSprite(miniGamesRT_Group[flipCardIndex].miniGameMaskRenderer, 
                                                               miniGamesRT_Group[flipCardIndex].miniGameMaskRenderer.material.GetFloat(DissolveRadiusName), 
                                                               0.35f, 2f));
        flipCardIndex ++;
    }
    public void OnStartFlipCard(Card card){
        miniGames[flipCardIndex].gameObject.SetActive(true);
        miniGames[flipCardIndex].StartMiniGame();
        miniGamesRT_Group[flipCardIndex].miniGameRoot.gameObject.SetActive(true);
    }
    public void UpdateChannelMask(float value){
        miniGameControl.channelWeight[flipCardIndex] = value*0.75f;
        miniGamesRT_Group[flipCardIndex].miniGameMaskRenderer.material.SetFloat(DissolveRadiusName, value*0.3f);
    }
    public Card GetCardByIndex(int index){
        return tarotGame.GetPlacedCardByIndex(index);
    }
}