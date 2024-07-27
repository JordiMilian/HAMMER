using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkSaw : MonoBehaviour
{
    [SerializeField] Transform position01, position02, sawTf;
    [SerializeField] float TimeToReach;
    [SerializeField] bool startTriggerSaw;
    [SerializeField] bool stopTriggerSaw;
    [Header("References")]
    [SerializeField] Animator sawAnimator;
    [SerializeField] Generic_AreaTriggerEvents sawAreaTrigger;
    int currentPos;
    private void OnEnable()
    {
        sawAreaTrigger.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        sawAreaTrigger.onAreaActive += startSawing;
        sawAreaTrigger.onAreaUnactive += stopSawing;
        currentPos = 1;
    }

    void startSawing()
    {
        sawAnimator.SetBool("isSawing", true);
    }
    void stopSawing()
    {
        sawAnimator.SetBool("isSawing", false);
    }
    public void EV_StartMoving()
    {
        if (currentPos == 1)
        {
            StartCoroutine(travelToPos(position01, position02, TimeToReach));
            currentPos = 2;
        }
        else if(currentPos == 2)
        {
            StartCoroutine(travelToPos(position02, position01, TimeToReach));
            currentPos = 1;
        }
    }
    void reachedEnd()
    {
        sawAnimator.SetTrigger("reachedEnd");
    }
    IEnumerator travelToPos(Transform initialPos, Transform finalPos, float timeToReach)
    {
        float timer = 0;
        float normalizedTime = 0;
        while(timer < timeToReach)
        {
            timer += Time.deltaTime;
            normalizedTime = timer / timeToReach;

            Vector2 newPos = Vector2.Lerp(initialPos.position, finalPos.position, BezierBlend(normalizedTime));

            sawTf.position = newPos;
            yield return null;
        }
        reachedEnd();
    }
    float BezierBlend(float t)
    {
        return t * t * (3.0f - 2.0f * t);
    }
    private void Update()
    {
        if(startTriggerSaw)
        {
            startSawing();
            startTriggerSaw = false;
        }
        if(stopTriggerSaw)
        {
            stopSawing();
            stopTriggerSaw = false;
        }
    }
}
