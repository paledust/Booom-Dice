using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMiniGameController : BasicMiniGameController
{
[Header("Person Control")]
    [SerializeField] private Transform personTrans;
    [SerializeField] private Transform personRotateRoot;
    [SerializeField] private float distanceToAngle;
[Space(20), Header("Cloud Spawn")]
    [SerializeField] private GameObject cloudPrefab;
    [SerializeField] private Transform spawnTrans;
    [SerializeField] private float spawnWidth = 2;
    [SerializeField] private float spawnRate = 5;
    [SerializeField] private int cloudPoolSize = 10;

    private int side = 1;
    private float depth;
    private CoroutineExcuter flipExcuter;
    private List<Cloud> cloudList;
    private float spawnTimer;

    void Awake()=>SetUp(targetCamera);
    void Start(){
        flipExcuter = new CoroutineExcuter(this);

        cloudList = new List<Cloud>();
        for(int i=0; i<cloudPoolSize; i++){
            var cloudObj = GameObject.Instantiate(cloudPrefab, transform);
            cloudObj.SetActive(false);
            cloudList.Add(cloudObj.GetComponent<Cloud>());
        }
        spawnTimer = Time.time;
    }
    void OnEnable(){
        depth = targetCamera.WorldToScreenPoint(personTrans.position).z;
    }
    void Update(){
        if(Time.time-spawnTimer>1f/spawnRate){
            spawnTimer = Time.time;
            cloudList.Find(x=>!x.gameObject.activeSelf)?.ActiveCloud(spawnTrans.position+Vector3.right*Random.Range(-spawnWidth/2f, spawnWidth/2f));
        }
    }
    public override void UpdateMiniGame(Vector3 pointerScreenPos)
    {
        Vector3 pos = GetWorldPointerPos(pointerScreenPos, depth);
        float diff = pos.z - personTrans.position.z;
        pos.y = personTrans.position.y;
        pos.z = personTrans.position.z;
        personTrans.position = pos;

        personRotateRoot.localRotation = Quaternion.Euler(0,0,distanceToAngle*diff);

        if(side > 0 && personTrans.localPosition.x < 0){
            side = -1;
            flipExcuter.Excute(coroutineFlipSize(-90, 0.5f));
        }
        if(side < 0 && personTrans.localPosition.x > 0){
            side = 1;
            flipExcuter.Excute(coroutineFlipSize(90, 0.5f));
        }
    }
    IEnumerator coroutineFlipSize(float targetAngle, float duration){
        float currentAngle = personTrans.localEulerAngles.x;
        yield return new WaitForLoop(duration, (t)=>{
            float angle = Mathf.Lerp(currentAngle, targetAngle, EasingFunc.Easing.QuadEaseOut(t));
            personTrans.localRotation = Quaternion.Euler(angle, 90, 90);
        });
    }
}
