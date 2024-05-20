using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PuddleCreatureLogiuc : MonoBehaviour
{
    Transform playerTf;
    [SerializeField] float maxChasingSpeed;
    [SerializeField] float chasingDuration;
    [SerializeField] float delayBetweenAttacks;
    [SerializeField] float delayWhenEntered;
    Animator puddleCreatureAnimator;
    Coroutine currentDelayBeforeChase;
    Coroutine currentChasingbeforeAttack;
    [SerializeField] List<Generic_OnTriggerEnterEvents> puddleTriggers = new List<Generic_OnTriggerEnterEvents>();
    private void Awake()
    {
        playerTf = GlobalPlayerReferences.Instance.playerTf;
        puddleCreatureAnimator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        foreach (Generic_OnTriggerEnterEvents ontrigger in puddleTriggers)
        {
            ontrigger.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
            ontrigger.OnTriggerEntered += onPlayerEnteredPuddle;
            ontrigger.OnTriggerExited += onPLayerExitedPuddle;
        }
    }
    void onPlayerEnteredPuddle(object sender, Generic_OnTriggerEnterEvents.EventArgsCollisionInfo info)
    {
        currentDelayBeforeChase = StartCoroutine(DelayToStartChase(delayWhenEntered));
    }
    void onPLayerExitedPuddle(object sender, Generic_OnTriggerEnterEvents.EventArgsCollisionInfo info)
    {
        cancelEverything();
        Debug.Log("player exited");
    }
    IEnumerator DelayToStartChase(float delay)
    {
        yield return new WaitForSeconds(delay);

        transform.position = playerTf.position;
        puddleCreatureAnimator.SetBool("Chasing",true);
        currentChasingbeforeAttack = StartCoroutine(ChasingCoroutine(chasingDuration));
    }

    IEnumerator ChasingCoroutine(float chasingDuration)
    {
        
        float timer = 0;
        while (timer < chasingDuration)
        {
            timer += Time.deltaTime;
            float inverseNormalizedTime = 1 -  (timer / chasingDuration);
            transform.position = Vector3.MoveTowards(transform.position, playerTf.position, maxChasingSpeed * inverseNormalizedTime * Time.deltaTime);
            yield return null;
        }
        AttemptAttack();
    }

    void AttemptAttack()
    {
        puddleCreatureAnimator.SetTrigger("Attack");
        puddleCreatureAnimator.SetBool("Chasing", false);
        currentDelayBeforeChase = StartCoroutine(DelayToStartChase(delayBetweenAttacks));
    }
    void cancelEverything()
    {
        puddleCreatureAnimator.SetBool("Chasing", false);
        if (currentDelayBeforeChase != null)
        {
            StopCoroutine(currentDelayBeforeChase);
        }
        if(currentChasingbeforeAttack != null)
        {
            StopCoroutine(currentChasingbeforeAttack);
        }
    }
}
