using System.Collections;
using System.Collections.Generic;
using SimpleAudioSystem;
using Unity.VisualScripting;
using UnityEngine;

public class HermitMiniGameController : BasicMiniGameController
{
[Header("LightSource Control")]
    [SerializeField] private Transform lightSource;
    [SerializeField] private float lerpSpeed = 10;
[Header("Tree Spawn")]
    [SerializeField] private int poolSize = 25;
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private GameObject[] treePrefabs;
    [SerializeField] private Vector2 spawnSizeRange;
    [SerializeField] private Vector2 spawnAngleRange;
    [SerializeField] private Transform spawnLeft;
    [SerializeField] private Transform spawnRight;
    [SerializeField] private float spawnWidth = 1;
    [SerializeField] private float spawnHeight = 0.2f;
[Space(20), Header("Audio")]
    [SerializeField] private AudioSource riverAudio;
    [SerializeField] private AudioSource stepAudio;
    [SerializeField] private AudioClip[] stepClips; 
    [SerializeField] private AudioClip[] lanternClips;
    [SerializeField] private float audioStep;
    private float depth;
    private MovingTree[] treeArray;
    private float spawnTimer;
    private float audioTimer;
    private int spawnIndex = 0;
    private int stepClipIndex = 0;

    void OnEnable(){
        depth = targetCamera.WorldToScreenPoint(lightSource.position).z;
    }
    public override void UpdateMiniGame(Vector3 pointerPos)
    {
    //Control Lantern
        Vector3 pos = GetWorldPointerPos(pointerPos, depth);
        pos.y = lightSource.position.y;
        lightSource.position = Vector3.Lerp(lightSource.position, pos, Time.deltaTime * lerpSpeed);

    //Spawn Tree
        if(spawnTimer+1/spawnRate<Time.time){
            spawnTimer = Time.time;
            SpawnATree(spawnIndex);
            spawnIndex ++;
            if(spawnIndex>=poolSize) spawnIndex = 0;
        }
    //Update Audio
        if(audioTimer + audioStep<Time.time){
            audioTimer = Time.time;
            AudioManager.Instance.PlaySoundEffect(stepAudio, stepClips[stepClipIndex], 1);
            stepClipIndex++;
            if(stepClipIndex>=stepClips.Length) stepClipIndex = 0;
        }
    }
    protected override void OnStart(){
        treeArray = new MovingTree[poolSize];
        for(int i=0; i<poolSize; i++){
            treeArray[i] = GameObject.Instantiate(treePrefabs[i%treePrefabs.Length], Vector3.zero, Quaternion.identity, transform).GetComponent<MovingTree>();
            treeArray[i].gameObject.SetActive(false);
        }
        Service.Shuffle(ref stepClips);
        Service.Shuffle(ref treeArray);
        riverAudio.Play();
    }
    void SpawnATree(int index){
        Transform spawnTrans = Random.Range(0,2)==0?spawnLeft:spawnRight;
        Vector3 spawnPos = spawnTrans.position;
        spawnPos += Vector3.right * Random.Range(-spawnWidth/2f, spawnWidth/2f) + Vector3.forward * Random.Range(-spawnHeight/2f, spawnHeight/2f);
        Quaternion spawnRot = Quaternion.Euler(90,0,spawnAngleRange.GetRndValueInVector2Range());

        treeArray[index].transform.position = spawnPos;
        treeArray[index].transform.rotation = spawnRot;
        treeArray[index].transform.localScale = Vector3.one * spawnSizeRange.GetRndValueInVector2Range();

        treeArray[index].gameObject.SetActive(true);
        treeArray[index].ResetState();
    }
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        Vector3 size = new Vector3(spawnWidth,0.01f,spawnHeight);
        Gizmos.DrawWireCube(spawnLeft.position, size);
        Gizmos.DrawWireCube(spawnRight.position, size);
    }
}
