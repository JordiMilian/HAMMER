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

    private void Awake()
    {
        genericChaserEvents.OnReceiveDamage += (object sender, Generic_EventSystem.ReceivedAttackInfo info) => BounceAway(info.ConcreteDirection, info.Attacker.transform);
        genericChaserEvents.OnDealtDamage += (object sender, Generic_EventSystem.DealtDamageInfo info) => DamagedPlayer();
        genericChaserEvents.OnGettingParried += (Generic_EventSystem.GettingParriedInfo info) => BounceAway((transform.position - info.Parrier.transform.position).normalized, info.Parrier.transform);
    }

    [Header("Chasing Stats")]
    [SerializeField] float startingVelocity;
    [SerializeField] float accelerationPerSecond;
    [SerializeField] float maxVelocity;
    [SerializeField] float startingRotation;
    [SerializeField] float rotationDecelerationPerSecond;
    public void startChasing (Transform target)
    {
        TargetTf = target;

        if (currentMovement != null) { StopCoroutine(currentMovement); }

        currentMovement = StartCoroutine(chaseCoroutine());
    }
    IEnumerator chaseCoroutine()
    {
        StartCoroutine(UsefullMethods.destroyWithDelay(10, gameObject));
        
        while (true)
        {
            startingVelocity += accelerationPerSecond * Time.deltaTime;
            if(rotationDecelerationPerSecond > 0)
            {
                startingRotation -= rotationDecelerationPerSecond * Time.deltaTime;
            }

            moveForward(startingVelocity);

            rotateTowardTarget(startingRotation);

            yield return null;
        } 
    }
    

    [Header("Bounce away Stats")]
    [SerializeField] float bounceDuration;
    [SerializeField] float bounceInitialSpeed;
    [SerializeField] float bounceInitialRotation;
    public void BounceAway(Vector2 directionAway, Transform target)
    {
        TargetTf = target;
        if (currentMovement != null) { StopCoroutine(currentMovement); }
        StartCoroutine(BounceAwayCoroutine(directionAway));

        damageDealer.EntityTeam = Generic_DamageDealer.Team.Player;
        damageDetector.GetComponent<Collider2D>().enabled = false;
    }
    
    IEnumerator BounceAwayCoroutine(Vector2 initialDirection)
    {
        transform.up = initialDirection;
        float timer = 0;
        while(timer < bounceDuration)
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
    public void DamagedPlayer()
    {
        Destroy(gameObject);
    }
    void rotateTowardTarget(float rotationMaxRadiant)
    {
        transform.up = Vector3.RotateTowards(transform.up, TargetTf.position - transform.position, rotationMaxRadiant, 10);
    }
    void moveForward(float Speed)
    {
        transform.position += transform.up * Speed * Time.deltaTime;
    }
}
