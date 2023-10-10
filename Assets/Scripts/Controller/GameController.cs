using System.Collections;
using System.Collections.Generic;
using TMPro;
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
[Header("Collect Words")]
    [SerializeField] private List<TextMeshPro> collectedWords;
[Header("Game Final")]
    [SerializeField] private TextMeshPro nameText;
    [SerializeField] private Animation maskAnime;
    [SerializeField] private Material textMaterial;
    [SerializeField] private Transform[] textPoses;
    public bool ReadingVision = false;

    private TarotTriangleMark currentVisionTriangle;
    private int flipCardIndex = 0;
    private Vector3 pointerPos;

    public const string DissolveRadiusName = "_DissolveRadius";
    private static readonly Vector3 miniGamePlacePos = new Vector3(0,0,1);
    private static readonly Vector3 miniGamePlaceRot = new Vector3(-90,0,0);
    void Start(){
        nameText.text = GameManager.Instance.StoredName+"\n的\n占卜结果";
    }
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
    public void LoadVisionTriangle(TarotTriangleMark visionTriangle){
        ReadingVision = true;
        currentVisionTriangle = visionTriangle;
    }
    public void FindTheWords(TextMeshPro text){
        int index = currentVisionTriangle.cardIndex;
        ReadingVision = false;
        currentVisionTriangle.OnFinishVision();
        currentVisionTriangle = null;

        TextMeshPro showingText = GameObject.Instantiate(text.gameObject).GetComponent<TextMeshPro>();
        showingText.color = new Color(1,1,1,0);
        showingText.transform.localScale = Vector3.one*0.44f;
        showingText.transform.position = textPoses[index].position;
        showingText.transform.rotation = text.transform.rotation;
        showingText.transform.localScale = Vector3.one * 0.15f;
        
        collectedWords.Add(showingText);
        if(collectedWords.Count == 3){
            StartCoroutine(coroutineEndGame(showingText));
        }
        else{
            StartCoroutine(coroutineBriefText(showingText, true));
        }
    }
    IEnumerator coroutineBriefText(TextMeshPro text, bool fadeOut){
        yield return CommonCoroutine.CoroutineFadeText(text, 1, 0.5f);
        if(fadeOut){
            yield return new WaitForSeconds(0.75f);
            yield return CommonCoroutine.CoroutineFadeText(text, 0, 3f);
            text.fontMaterial = textMaterial;
            text.color = Color.white;
        }
    }
    IEnumerator coroutineEndGame(TextMeshPro finalText){
        yield return CommonCoroutine.CoroutineFadeText(finalText, 1, 0.5f);
        StartCoroutine(CommonCoroutine.CoroutineChangeTransSize(finalText.transform, Vector3.one*0.15f, 3f));
        yield return new WaitForSeconds(1f);
        Vector3 maskPos = finalText.transform.position;
        maskPos.y = maskAnime.transform.position.y;
        maskAnime.gameObject.SetActive(true);
        maskAnime.Play();
        yield return new WaitForSeconds(maskAnime.clip.length);
        yield return new WaitForSeconds(7f);
        GameManager.Instance.EndGame();
    }
}