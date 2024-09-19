using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.VFX;

public class PuddleCreatureLogiuc : MonoBehaviour
{
    Transform playerTf;
    [SerializeField] float maxChasingSpeed;
    public float chasingDuration;
    [SerializeField] float delayBetweenAttacks;
    [SerializeField] float delayWhenEntered;
    Animator puddleCreatureAnimator;
    Coroutine currentDelayBeforeChase;
    Coroutine currentChasingbeforeAttack;
    [SerializeField] List<Generic_OnTriggerEnterEvents> puddleTriggers = new List<Generic_OnTriggerEnterEvents>();
    VisualEffect BigStepVFX;

    [SerializeField] RoomWithEnemiesLogic roomWithEnemies;
    private void Awake()
    {
        playerTf = GlobalPlayerReferences.Instance.playerTf;
        puddleCreatureAnimator = GetComponent<Animator>();
        BigStepVFX = GetComponent<VisualEffect>();
    }
    private void OnEnable()
    {
        foreach (Generic_OnTriggerEnterEvents ontrigger in puddleTriggers)
        {
            ontrigger.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
            ontrigger.OnTriggerEntered += onPlayerEnteredPuddle;
            ontrigger.OnTriggerExited += onPLayerExitedPuddle;
        }
        roomWithEnemies.onRoomCompleted += (BaseRoomWithDoorLogic doorLogic) => DeactivatePuddle();
    }
    void onPlayerEnteredPuddle(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(TagsCollection.Player_SinglePointCollider))
        {
            currentDelayBeforeChase = StartCoroutine(DelayToStartChase(delayWhenEntered));
        } 
    }
    void onPLayerExitedPuddle(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(TagsCollection.Player_SinglePointCollider))
        {
            cancelEverything();
        }
        
    }
    void DeactivatePuddle()
    {
        cancelEverything();
        foreach (Generic_OnTriggerEnterEvents ontrigger in puddleTriggers)
        {
            ontrigger.OnTriggerEntered -= onPlayerEnteredPuddle;
            ontrigger.OnTriggerExited -= onPLayerExitedPuddle;
        }
    }
    IEnumerator DelayToStartChase(float delay)
    {
        yield return new WaitForSeconds(delay);

        transform.position = playerTf.position;
        puddleCreatureAnimator.SetBool("Chasing",true);
        currentChasingbeforeAttack = StartCoroutine(ChasingCoroutine(chasingDuration));
    }

    public IEnumerator ChasingCoroutine(float chasingDuration)
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
        BigStepVFX.Play();
        CameraShake.Instance.ShakeCamera(.5f, 0.4f);
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
