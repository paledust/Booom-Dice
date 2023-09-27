using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandController : MonoBehaviour
{
[Header("Hand Move")]
    [SerializeField] private Transform handTarget;
    [SerializeField] private float lerpSpeed = 10;
[Header("Hand Interact")]
    [SerializeField] private PointClick_InteractableHandler pointClick_InteractableHandler;

    private float depth;
    private Camera mainCam;

    void Start(){
        Cursor.lockState = CursorLockMode.Confined;
        mainCam = Camera.main;
        depth = mainCam.WorldToScreenPoint(handTarget.position).z;
    }
    public void Hand_Update(Vector3 pointer){
        UpdateHandPos(pointer);
        pointClick_InteractableHandler.DetectInteractable();
    }
    public void Hand_Interact(bool isPressed){
        pointClick_InteractableHandler.InteractWithInteractable(isPressed);
    }
    void UpdateHandPos(Vector3 pointer)
    {
        pointer.z = depth;
        Vector3 targetPosition = mainCam.ScreenToWorldPoint(pointer);
        targetPosition.y = handTarget.position.y;

        handTarget.position = Vector3.Lerp(handTarget.position, targetPosition, Time.deltaTime*lerpSpeed);
    }
}
