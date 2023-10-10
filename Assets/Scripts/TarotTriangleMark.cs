using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TarotTriangleMark : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float overAllAlpha = 1;
    [SerializeField] private Vector2 detectCenter;
    [SerializeField] private float offsetToAlpha;
    [SerializeField] private float offsetToPos;
    [SerializeField] private Camera targetCam;
    private SpriteRenderer[] triangleRenderers;
    private Color[] initColors;
    private Vector3[] initPos;
    private Vector2 offset;
    void Start(){
        triangleRenderers = GetComponentsInChildren<SpriteRenderer>();
        initColors = new Color[triangleRenderers.Length];
        initPos = new Vector3[triangleRenderers.Length];
        for(int i=0; i<initColors.Length; i++){
            initPos[i] = triangleRenderers[i].transform.localPosition;
            initColors[i] = triangleRenderers[i].color;
        }
    }
    void Update(){
        UpdateTriangle(Vector2.zero);
    }
    void UpdateTriangle(Vector2 pointerPos){
        Vector2 offset = detectCenter;
        for(int i=0; i<triangleRenderers.Length; i++){
            triangleRenderers[i].transform.localPosition = initPos[i] + new Vector3(offset.x, offset.y, 0)*offsetToPos*i;
            Color color = initColors[i];
            color.a = Mathf.Lerp(0, initColors[i].a, Mathf.Clamp01(1-offset.magnitude/offsetToAlpha));
            triangleRenderers[i].color = color;
        }
    }
}
