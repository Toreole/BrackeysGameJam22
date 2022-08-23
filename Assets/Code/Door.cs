using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private bool isLocked = true;
    [SerializeField]
    private float openLocal, closedLocal; //local Y positions.
    [SerializeField]
    private Transform posSide, negSide;
    [SerializeField]
    private float duration;
    [SerializeField]
    private AnimationCurve animCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField]
    private Lock mLock;

    private bool isOpen = false;
    private int ccount = 0;

    private void OnEnable()
    {
        if(mLock)
            mLock.OnUnlock += Unlock;
    }

    private void OnDisable()
    {
        if(mLock)
            mLock.OnUnlock -= Unlock;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (isLocked)
            return;
        if(Eval(collider))
        {
            Debug.Log("enter opener", collider);
            ccount++;
            isOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (isLocked)
            return;
        if (Eval(collider))
        {
            ccount--;
            if (ccount == 0)
            {
                isOpen = false;
                Debug.Log("close now");
            }
        }
    }

    private void LateUpdate()
    {
        Vector3 posLocal = posSide.localPosition;
        //inverse t (progress)
        float inverse_t = Mathf.InverseLerp(closedLocal, openLocal, posLocal.y);
        //other direction depending on open state. step scaled by duration.
        float step = (isOpen? 1f : -1f) * Time.deltaTime / duration;
        float t = Mathf.Clamp01(inverse_t + step);
        //curve t
        //float ct = animCurve.Evaluate(t);
        //lerp result
        float y = Mathf.Lerp(closedLocal, openLocal, t);
        //assign y
        posLocal.y = y;
        posSide.localPosition = posLocal;
        negSide.localPosition = -posLocal;
    }

    private void Unlock()
    {
        isLocked = false;
    }

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private bool Eval(Collider2D collider) 
        => collider.isTrigger == false && (collider.CompareTag("Player") || collider.CompareTag("Friendly NPC"));
}
