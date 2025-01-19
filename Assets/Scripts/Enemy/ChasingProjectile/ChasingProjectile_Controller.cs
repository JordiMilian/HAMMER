using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingProjectile_Controller : MonoBehaviour
{
    [HideInInspector] public Transform TargetTf;
    [SerializeField] Generic_EventSystem genericChaserEvents;
    Coroutine currentMovement;
    [SerializeField] Generic_DamageDealer damageDealer;
    [SerializeField] Generic_DamageDetector damageDetector;
    [SerializeField] AudioClip spawnAudioClip;
     Transform originalSender;

    private void Awake()
    {
        genericChaserEvents.OnReceiveDamage +=DamagedProjectile;
        genericChaserEvents.OnDealtDamage += DamagedPlayer;
        genericChaserEvents.OnGettingParried += ParriedProjectile;
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
    public void SpawnAndRotateAround()
    {

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
    
  
   
    [Header("Damaged Stats")]
    [SerializeField] float bounceDuration;
    [SerializeField] float bounceInitialSpeed;
    [SerializeField] float bounceInitialRotation;
    void DamagedProjectile( object sender, Generic_EventSystem.ReceivedAttackInfo info)
    {
        if (currentMovement != null) { StopCoroutine(currentMovement); }
        StartCoroutine(BounceAwayCoroutine(info.ConcreteDirection));

        damageDealer.EntityTeam = Generic_DamageDealer.Team.Player;
        damageDetector.GetComponent<Collider2D>().enabled = false;

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
    void ParriedProjectile(Generic_EventSystem.GettingParriedInfo info)
    {
        TargetTf = originalSender;
        originalSender = info.Parrier.transform;
        

        damageDealer.EntityTeam = Generic_DamageDealer.Team.Player;
        damageDetector.EntityTeam = Generic_DamageDetector.Team.Player;

        transform.up = info.ParryDirection;
        startChasing();
    }
    void DamagedPlayer(object sender, Generic_EventSystem.DealtDamageInfo info)
    {
        Destroy(gameObject);
    }
   
}
