using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.VFX;

public class DeadPartV3 : MonoBehaviour
{
    [SerializeField] Rigidbody2D DeadPart_RB;
    [SerializeField] List<Rigidbody2D> ChildDeadParts = new List<Rigidbody2D>();
    public Transform movingParent;
    [SerializeField] Collider2D groundCollider;
    //public Generic_OnTriggerEnterEvents triggerDetector;//DAMAGE DETECTOR PLS
    [SerializeField] Generic_DamageDetector damageDetector;
    [HideInInspector] public bool isPushed;

    [SerializeField] DeadPart_EventSystem eventSystem;



    [Header("Shared Stats")]
    [SerializeField] AnimationCurve groundAnimationCurve;
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
    Vector2 currentDirection;
    public Coroutine currentPush;

    private void OnEnable()
    {
        //Subscribe to everyting
        eventSystem.OnSpawned += SpawnedPush;
        eventSystem.OnReceiveDamage += AttackPush;
        eventSystem.OnBeingTouchedObject += TouchedPush;
        eventSystem.OnHitWall += HitWallPush;
        
        //Get the ground and add it to the list of the manager
        DeadParts_Manager.Instance.GroundsList.Add(groundCollider);

        //Add every DeadPart collider to a List
        DeadPart_relatedColliders.Add(DeadPart_RB.GetComponent<Collider2D>());
        foreach (Rigidbody2D rb in ChildDeadParts)
        {
            DeadPart_relatedColliders.Add(rb.GetComponent<Collider2D>());
        }
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
        eventSystem.OnBeingTouchedObject -= TouchedPush;
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
        DeadPart_RB.position = DeadPart_RB.position + parentVelocity;

    }
    void SpawnedPush(object sender, Generic_EventSystem.ObjectDirectionArgs args)
    {
        Debug.Log("spawn pushed");
        CallVertical(args.GeneralDirection, 1f, 1f);
    }
    void AttackPush(object sender, Generic_EventSystem.ReceivedAttackInfo args)
    {
        CallHorizontal(args.GeneralDirection, 1f, 1f);
    }
    void TouchedPush(object sender, Generic_EventSystem.ObjectDirectionArgs args)
    {
        CallHorizontal(args.GeneralDirection, 0.2f, 0.5f);
    }
    void HitWallPush()
    {
        damageDetector.enabled = true;
        CallHorizontal(-currentDirection, 0.4f, 1f);
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
     IEnumerator PushCoroutine( Vector2 direction, float intencity, float duration, float maxRotation, float verticalForce, float distance, float random)
    {
        //Wait pa que se instancie tot be
        yield return new WaitForSeconds(0.05f);

        float timer = 0;
        float randomTorque = UnityEngine.Random.Range(-maxRotation, maxRotation);
        Vector2 initialPosition = movingParent.position;


        //Get a random direction 
        Vector2 randomDirection = Quaternion.Euler(0, 0, Random.Range(random, -random)) * direction;
        currentDirection = randomDirection;
        DeadPart_RB.AddTorque(randomTorque*10);

        DeadPart_RB.AddForce(Vector2.up * verticalForce * intencity*10);
        while (timer < duration)
        {
            isPushed = true;
            timer += Time.deltaTime * Time.timeScale;

            //find 
            float normalizedTime = Mathf.InverseLerp(0, duration, timer);
            float groundCurvePoint = groundAnimationCurve.Evaluate(normalizedTime);

            //Move the ground
            movingParent.position = initialPosition + (randomDirection * groundCurvePoint * intencity * distance);
            //Debug.Log((randomDirection * groundCurvePoint * intencity * Time.timeScale * groundDistance));
            
           
            //At the beggining, add upward force and rotate the DeadPart
            if (timer < duration / 8)
            {
                float editedTimeScale = Mathf.Lerp(0.5f, 1, Time.timeScale);
                //Debug.Log(Time.timeScale + " -> " + editedTimeScale);
                verticalForce = Mathf.Lerp(verticalForce, 0, 0.03f);
                
            }
            //else if(timer < pushDuration / 8 * 2) { triggerDetector.GetComponent<Collider2D>().enabled = true; }

            yield return null;
        }
        damageDetector.enabled = true;
        isPushed = false;
    }
    
    
}
