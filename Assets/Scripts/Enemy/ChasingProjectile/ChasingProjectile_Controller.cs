using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingProjectile_Controller : MonoBehaviour , IDamageDealer, IDamageReceiver, IParryReceiver
{
    [HideInInspector] public Transform TargetTf;
    [SerializeField] Generic_EventSystem genericChaserEvents;
    Coroutine currentMovement;
    [SerializeField] Generic_DamageDealer damageDealer;
    [SerializeField] Generic_DamageDetector damageDetector;
    [SerializeField] AudioClip spawnAudioClip;
     Transform originalSender;
    [Header("Damaged Stats")]
    [SerializeField] float bounceDuration;
    [SerializeField] float bounceInitialSpeed;
    [SerializeField] float bounceInitialRotation;

    public Action<DealtDamageInfo> OnDamageDealt_event { get; set; }
    public void OnDamageDealt(DealtDamageInfo info)
    {
        Destroy(gameObject);
        OnDamageDealt_event?.Invoke(info);
    }
    public Action<ReceivedAttackInfo> OnDamageReceived_Event { get; set; }
    public void OnDamageReceived(ReceivedAttackInfo info)
    {
        if (currentMovement != null) { StopCoroutine(currentMovement); }
        StartCoroutine(BounceAwayCoroutine(info.CollidersDirection));

        damageDealer.EntityTeam = DamagersTeams.Player;
        damageDetector.GetComponent<Collider2D>().enabled = false;

        OnDamageReceived_Event?.Invoke(info);
        //
        IEnumerator BounceAwayCoroutine(Vector2 initialDirection)
        {
            transform.up = initialDirection;
            float timer = 0;
            while (timer < bounceDuration)
            {
                timer += Time.deltaTime;
                float normalizedTime = timer / bounceDuration;
                float thisVelocity = Mathf.Lerp(bounceInitialSpeed, 0, normalizedTime);
                float thisRotation = Mathf.Lerp(bounceInitialRotation, 0, normalizedTime);

                rotateTowardTarget(thisRotation);
                moveForward(thisVelocity);

                yield return null;
            }
            Destroy(gameObject);
        }
    }

    public Action<GettingParriedInfo> OnParryReceived_event { get; set; }
    public void OnParryReceived(GettingParriedInfo info)
    {
        TargetTf = originalSender;
        originalSender = info.Parrier.transform;


        damageDealer.EntityTeam = DamagersTeams.Player;
        damageDetector.EntityTeam = DamagersTeams.Player;

        transform.up = info.ParryDirection;
        startChasing();
        OnParryReceived_event?.Invoke(info);
    }
    void rotateTowardTarget(float rotationMaxRadiant)
    {
        if (TargetTf == null) { return; }
        transform.up = Vector3.RotateTowards(transform.up, TargetTf.position - transform.position, rotationMaxRadiant, 10);
    }
    void moveForward(float Speed)
    {
        transform.position += transform.up * Speed * Time.deltaTime;
    }

    [Header("Chasing Stats")]
    [SerializeField] float startingVelocity;
    [SerializeField] float accelerationPerSecond;
    [SerializeField] float maxVelocity;
    [SerializeField] float startingRotation;
    [SerializeField] float rotationDecelerationPerSecond;
    [SerializeField] float defaultDelayBeforeChase;
    public void SpawnDelayAndChase(Vector2 startingDirection, Transform target, Transform sender)
    {
        originalSender = sender;
        TargetTf = target;
        transform.up = startingDirection;

        if (currentMovement != null) { StopCoroutine(currentMovement); }
        currentMovement = StartCoroutine(defaultDelay());

        //
        IEnumerator defaultDelay()
        {
            yield return new WaitForSeconds(defaultDelayBeforeChase);
            startChasing();
        }
    }
     void startChasing()
    {
        if(currentMovement != null) { StopCoroutine(currentMovement); }
        currentMovement = StartCoroutine(chaseCoroutine());

        //
        IEnumerator chaseCoroutine()
        {
            StartCoroutine(UsefullMethods.destroyWithDelay(10, gameObject));
            float currentVelocity = startingVelocity;
            while (true)
            {
                currentVelocity += accelerationPerSecond * Time.deltaTime;
                if (rotationDecelerationPerSecond > 0)
                {
                    startingRotation -= rotationDecelerationPerSecond * Time.deltaTime;
                }

                moveForward(currentVelocity);

                rotateTowardTarget(startingRotation);

                yield return null;
            }
        }
    }


}
