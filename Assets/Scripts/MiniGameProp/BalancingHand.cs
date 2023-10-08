using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancingHand : MonoBehaviour
{
    [SerializeField] private Transform leftHandTrans;
    [SerializeField] private Transform rightHandTrans;
    [SerializeField, Range(-1, 1)] private float balanceValue = 0;
    [SerializeField] private float balanceToHeight = 2;
    [SerializeField] private float lerpSpeed = 5;
    private float balanceHeight;
    private Vector3 initLeftHandPos;
    private Vector3 initRightHandPos;
    void Start(){
        initLeftHandPos = leftHandTrans.localPosition;
        initRightHandPos = rightHandTrans.localPosition;
    }
    // Update is called once per frame
    void Update()
    {
        balanceHeight = Mathf.Lerp(balanceHeight, balanceValue*balanceToHeight, Time.deltaTime*lerpSpeed);
        leftHandTrans.localPosition = initLeftHandPos + balanceHeight*Vector3.forward;
        rightHandTrans.localPosition = initRightHandPos - balanceHeight*Vector3.forward;
    }
}
