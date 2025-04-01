using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeadPartV3_Controller : MonoBehaviour, IDamageReceiver
{
    //AL DEAD PART ROOT LI FIQUES UN CHARACTER MOVER I A PENDRE PEL CUL

    [SerializeField] Rigidbody2D simulatedRigidBody;
    [SerializeField] List<Rigidbody2D> ChildDeadParts = new List<Rigidbody2D>();
    public Transform movingParent;
    [SerializeField] Collider2D groundCollider;
    //public Generic_OnTriggerEnterEvents triggerDetector;//DAMAGE DETECTOR PLS
    [SerializeField] Collider2D damageDetector;
    [SerializeField] Animator animator_DeadPart;
    [SerializeField] Generic_Flash flasher;



    [Header("Shared Stats")]
    [SerializeField] AnimationCurve groundAnimationCurve;
    [SerializeField] float averageOfCurve;
    [Header("Horizontal Stats")]
    [SerializeField] float hor_Duration;
    [SerializeField] float hor_maxRandomRotation;
    [SerializeField] float hor_verticalForce;
    [SerializeField] float hor_groundDistance;
    [SerializeField] float hor_directionRandom;

    [Header("Vertical Stats")]
    [SerializeField] float ver_Duration;
    [SerializeField] float ver_maxRandomRotation;
    [SerializeField] float ver_verticalForce;
    [SerializeField] float ver_groundDistance;
    [SerializeField] float ver_directionRandom;



    List<Collider2D> DeadPart_relatedColliders = new List<Collider2D>();
    Vector2 previusParentPosition;
    public Coroutine currentPush;

    public System.Action<ReceivedAttackInfo> OnDamageReceived_event { get ; set; }

    private void OnEnable()
    {
        averageOfCurve = UsefullMethods.GetAverageValueOfCurve(groundAnimationCurve, 10);

        //Get the ground and add it to the list of the manager
        DeadParts_Manager.Instance.GroundsList.Add(groundCollider);

        //Add every DeadPart collider to a List
        DeadPart_relatedColliders.Add(simulatedRigidBody.GetComponent<Collider2D>());
        foreach (Rigidbody2D rb in ChildDeadParts)
        {
            DeadPart_relatedColliders.Add(rb.GetComponent<Collider2D>());
        }

        // Destroy when player respawns
        GameEvents.OnPlayerRespawned += DestroyItself;

        //Subscribe and Invoke the Event that calls everyone to revisit what colliders to ignore 
        DeadParts_Manager.Instance.OnDeadPartInstantiated += IgnoreOtherGrounds;
        DeadParts_Manager.Instance.OnDeadPartInstantiated?.Invoke();
    }
 
    void IgnoreOtherGrounds()
    {
        //Make every Collider listed to Ignore every Ground except its own
        foreach (Collider2D col in DeadPart_relatedColliders)
        {
            DeadParts_Manager.Instance.IgnoreAllGroundExceptThis(groundCollider, col);
        }
    }
    private void OnDisable()
    {
        DeadParts_Manager.Instance.GroundsList.Remove(groundCollider);
        DeadParts_Manager.Instance.OnDeadPartInstantiated -= IgnoreOtherGrounds;
    }
    private void Start()
    {
        previusParentPosition = movingParent.position;
    }
    private void FixedUpdate()
    {
        //Get velocity and reset prev position
        Vector2 parentPosition = movingParent.position;
        Vector2 parentVelocity = parentPosition - previusParentPosition;
        previusParentPosition = parentPosition;
        
        //Add the velocity position to the non-simulated RB
        simulatedRigidBody.position += parentVelocity;

    }
    public void OnSpawnedPush()
    {
        //When spawned, push to random direction.
        //This is called from the instantiator, so we have time to position the head properly
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(-1, 1);
        Vector2 randomDirection = new Vector2(randomX, randomY);
        randomDirection.Normalize();
        CallVertical(randomDirection, 1, 1);

        flasher.CallDefaultFlasher();
        animator_DeadPart.SetTrigger("Strong");

    }
    public void OnDamageReceived(ReceivedAttackInfo info)
    {
        TimeScaleEditor.Instance.HitStop(IntensitiesEnum.Small);
        CameraShake.Instance.ShakeCamera(IntensitiesEnum.Small);
        GroundBloodPlayer.Instance.PlayGroundBlood(groundCollider.transform.position, info.CollidersDirection);
        animator_DeadPart.SetTrigger("Strong");

        CallHorizontal(info.CollidersDirection, 1, 1);
        OnDamageReceived_event?.Invoke(info);
    }
    public void DestroyItself()
    {
        Destroy(gameObject);
    }

    public void CallHorizontal(Vector2 direction, float intencity, float durationMultiplier)
    {
        damageDetector.enabled = false;
        if (currentPush != null) { StopCoroutine(currentPush); }
        currentPush = StartCoroutine(PushCoroutine(
            direction, 
            intencity,
            hor_Duration * durationMultiplier,
            hor_maxRandomRotation,
            hor_verticalForce,
            hor_groundDistance,
            hor_directionRandom
            ));
    }
   
    public void CallVertical(Vector2 direction, float intencity, float durationMultiplier)
    {
        damageDetector.enabled = false;

        if (currentPush != null) { StopCoroutine(currentPush); }
        currentPush = StartCoroutine(PushCoroutine(
            direction,
            intencity,
            ver_Duration * durationMultiplier,
            ver_maxRandomRotation,
            ver_verticalForce,
            ver_groundDistance,
            ver_directionRandom
            ));
    }
    [SerializeField] Generic_CharacterMover groundMover;
     IEnumerator PushCoroutine( Vector2 direction, float intencity, float duration, float maxRotation, float verticalForce, float distance, float directionRandomness)
    {
        //Wait pa que se instancie tot be
        yield return new WaitForSeconds(0.05f);

        float randomTorque = UnityEngine.Random.Range(-maxRotation, maxRotation);
        Vector2 randomDirection = Quaternion.Euler(0, 0, Random.Range(directionRandomness, -directionRandomness)) * direction;

        simulatedRigidBody.AddTorque(randomTorque*10);

        simulatedRigidBody.AddForce(Vector2.up * verticalForce * intencity*10);

        yield return StartCoroutine(UsefullMethods.ApplyCurveMovementOverTime(groundMover, distance, duration, randomDirection, groundAnimationCurve, averageOfCurve));

        damageDetector.enabled = true;
    }

    
}
