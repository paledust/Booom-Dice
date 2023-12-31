using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookableSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] lookablePrefabs;
    [SerializeField] private int poolSizeForEach;
    [SerializeField] private float spawnWidth;
    [SerializeField] private float spawnRate;
    private int spawnIndex = 0;
    private float spawnTimer;
    [SerializeField]private Lookable[] lookables;
    void Start()
    {
        lookables = new Lookable[lookablePrefabs.Length*poolSizeForEach];
        for(int i=0; i<lookablePrefabs.Length; i++){
            for(int k=0; k<poolSizeForEach; k++){
                var lookable = GameObject.Instantiate(lookablePrefabs[i], transform).GetComponent<Lookable>();
                lookables[i*poolSizeForEach+k] = lookable;
                lookable.gameObject.SetActive(false);
            }
        }
        Service.Shuffle(ref lookables);
    }

    void Update()
    {
        if(spawnTimer + 1/spawnRate < Time.time){
            spawnTimer = Time.time;
            lookables[spawnIndex].gameObject.SetActive(true);
            lookables[spawnIndex].OnReset(transform.position + Vector3.right * Random.Range(-spawnWidth/2f, spawnWidth/2f));
            spawnIndex ++;
            if(spawnIndex==lookables.Length) spawnIndex = 0;
        }
    }

}
