using System.Collections;
using System.Collections.Generic;
using AmplifyShaderEditor;
using UnityEngine;

public class MovingTree : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private Transform[] mushroomGrowTrans;
    [SerializeField] private MushRoom[] mushRooms;
    public bool m_growed{get; private set;} = false;
    void Update(){
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
    }
    public void GrowMushRoom(){
        m_growed = true;
        Service.Shuffle(ref mushroomGrowTrans);
        StartCoroutine(coroutineMushroomGrow());
    }
    public void ResetState(){
        m_growed = false;
        for(int i=0; i<mushRooms.Length; i++){
            mushRooms[i].gameObject.SetActive(false);
        }
    }
    IEnumerator coroutineMushroomGrow(){
        int mushRoomCount = Random.Range(1, mushRooms.Length);
        for(int i=0; i<=mushRoomCount; i++){
            mushRooms[i].gameObject.SetActive(true);
            mushRooms[i].MushRoomGrow(mushroomGrowTrans[i]);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }
    }
}
