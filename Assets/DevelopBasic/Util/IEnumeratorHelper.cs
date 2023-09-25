using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineExcuter
{
    MonoBehaviour initiator;
    IEnumerator coroutine;
    public CoroutineExcuter(MonoBehaviour _context)=>initiator = _context;
    public void Excute(IEnumerator go){
        if(coroutine!=null) initiator.StopCoroutine(coroutine);
        coroutine = go;
        initiator.StartCoroutine(go);
    }
    public void Abort(){
        if(coroutine!=null) initiator.StopCoroutine(coroutine);
    }
}

public class WaitForLoop: IEnumerator
{
    public delegate void LoopMain(float t);
    private IEnumerator m_coroutine;

    public WaitForLoop(float _duration, LoopMain _go){
        m_coroutine = ForLoopCoroutine(_duration, _go);
    }

    public object Current{get{return m_coroutine.Current;}}
    public bool MoveNext(){return m_coroutine.MoveNext();}
    public void Reset()=>m_coroutine.Reset();
    
    public static IEnumerator ForLoopCoroutine(float duration, LoopMain go){
        float speed = 1f/duration;
        for(float t=0; t<1; t+=Time.deltaTime*speed){
            go(t);
            yield return null;
        }
        go(1);
    }
}