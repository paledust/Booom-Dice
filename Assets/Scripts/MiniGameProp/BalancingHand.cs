using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancingHand : MonoBehaviour
{
    [SerializeField] private Transform leftHandTrans;
    [SerializeField] private Transform rightHandTrans;
    [SerializeField, Range(-1, 1)] private float balanceValue = 0;
    [SerializeField] private AnimationCurve easeCurve;
    [SerializeField] private float balanceToHeight = 2;
[Header("Animation")]
    [SerializeField] private Animator leftHandAnime;
    [SerializeField] private Animator rightHandAnime;

    private float balanceHeight;
    private Vector3 initLeftHandPos;
    private Vector3 initRightHandPos;
    private CoroutineExcuter weightChanger;

    private const string OffsetFloatName = "Offset";

    void Start(){
        weightChanger = new CoroutineExcuter(this);

        initLeftHandPos = leftHandTrans.localPosition;
        initRightHandPos = rightHandTrans.localPosition;

        leftHandAnime.SetFloat(OffsetFloatName, Random.Range(0f,1f));
        rightHandAnime.SetFloat(OffsetFloatName, Random.Range(0f,1f));
    }
    // Update is called once per frame
    void Update()
    {
        leftHandTrans.localPosition = initLeftHandPos + balanceHeight*Vector3.forward;
        rightHandTrans.localPosition = initRightHandPos - balanceHeight*Vector3.forward;
    }
    public void AddToRightHand(float weight){
        if(balanceValue>=1){
            balanceValue += weight;
            weightChanger.Excute(coroutineShakeHand(balanceValue*balanceToHeight, 1f));
            balanceValue = Mathf.Clamp(balanceValue,-1,1);
        }
        else{
            balanceValue += weight;
            weightChanger.Excute(coroutineChangeBalanceValue(balanceValue*balanceToHeight, 1f));
            balanceValue = Mathf.Clamp(balanceValue,-1,1);
        }
    }
    public void AddToLeftHand(float weight){
        if(balanceValue<=-1){
            balanceValue -= weight;
            weightChanger.Excute(coroutineShakeHand(balanceValue*balanceToHeight, 1f));
            balanceValue = Mathf.Clamp(balanceValue,-1,1);
        }
        else{
            balanceValue -= weight;
            balanceValue = Mathf.Clamp(balanceValue,-1,1);
            weightChanger.Excute(coroutineChangeBalanceValue(balanceValue*balanceToHeight, 1f));
        }
    }
    IEnumerator coroutineChangeBalanceValue(float targetWeight, float duration){
        float initWeight = balanceHeight;
        yield return new WaitForLoop(duration, (t)=>{
            balanceHeight = Mathf.LerpUnclamped(initWeight, targetWeight, easeCurve.Evaluate(t));
        });
    }
    IEnumerator coroutineShakeHand(float targetWeight, float duration){
        float initWeight = balanceHeight;
        yield return new WaitForLoop(duration, (t)=>{
            balanceHeight = Mathf.LerpUnclamped(initWeight, targetWeight, EasingFunc.Easing.CosPulse(t));
        });
    }
}
