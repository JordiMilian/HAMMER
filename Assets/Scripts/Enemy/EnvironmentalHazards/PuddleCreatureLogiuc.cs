using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.VFX;

public class PuddleCreatureLogiuc : MonoBehaviour, IDamageDealer
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
    [SerializeField] AudioClip CreatureSFX;

    [SerializeField] GameObject EnemyRoom_InterfaceHolder;
    IRoomWithEnemies EnemyRoomsInterface;

    public Action<DealtDamageInfo> OnDamageDealt_event { get; set ; }

    private void Awake()
    {
        OnValidate();
        playerTf = GlobalPlayerReferences.Instance.playerTf;
        puddleCreatureAnimator = GetComponent<Animator>();
        BigStepVFX = GetComponent<VisualEffect>();
    }
    private void OnValidate()
    {
        UsefullMethods.CheckIfGameobjectImplementsInterface<IRoomWithEnemies>(ref EnemyRoom_InterfaceHolder, ref EnemyRoomsInterface);
    }
    private void OnEnable()
    {
        foreach (Generic_OnTriggerEnterEvents ontrigger in puddleTriggers)
        {
            ontrigger.AddActivatorTag(Tags.Player_SinglePointCollider);
            ontrigger.OnTriggerEntered += onPlayerEnteredPuddle;
            ontrigger.OnTriggerExited += onPLayerExitedPuddle;
        }
        EnemyRoomsInterface.OnAllEnemiesKilled += DeactivatePuddle;


    }
    void onPlayerEnteredPuddle(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(Tags.Player_SinglePointCollider))
        {
            currentDelayBeforeChase = StartCoroutine(DelayToStartChase(delayWhenEntered));
        } 
    }
    void onPLayerExitedPuddle(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(Tags.Player_SinglePointCollider))
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
        CameraShake.Instance.ShakeCamera(IntensitiesEnum.Big);
        SFX_PlayerSingleton.Instance.playSFX(CreatureSFX);
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

    public void OnDamageDealt(DealtDamageInfo info)
    {
        OnDamageDealt_event?.Invoke(info);
    }
}
