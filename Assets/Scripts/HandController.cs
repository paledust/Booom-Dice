using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandController : MonoBehaviour
{
    [SerializeField] private Transform handTarget;
    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private float lerpSpeed = 10;

    private float depth;
    private Camera mainCam;

    void Start(){
        mainCam = Camera.main;
        depth = mainCam.WorldToScreenPoint(handTarget.position).z;
    }

    public void UpdateInput(Vector3 pointer)
    {
        pointer.z = depth;
        Vector3 targetPosition = mainCam.ScreenToWorldPoint(pointer);
        targetPosition.y = handTarget.position.y;

        handTarget.position = Vector3.Lerp(handTarget.position, targetPosition, Time.deltaTime*lerpSpeed);
    }
}
