using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.VFX;

public class DeadPartV3_Min : MonoBehaviour
{
    [SerializeField] Rigidbody2D DeadPart_RB;
    [SerializeField] List<Rigidbody2D> ChildDeadParts = new List<Rigidbody2D>();
    public Transform movingParent;
    [SerializeField] Collider2D groundCollider;
    public Generic_OnTriggerEnterEvents triggerDetector;
    [HideInInspector] public bool isPushed;

    [SerializeField] DeadPart_EventSystem_min eventSystem;



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
    [Header("Debug stuff (Press E to Test)")]
    [SerializeField] bool doVertical;
    [SerializeField] Vector2 debugDirection;
    [SerializeField] float debugIntensity;



    List<Collider2D> DeadPart_relatedColliders = new List<Collider2D>();
    Vector2 previusParentPosition;
    Vector2 currentDirection;
    public Coroutine currentPush;

    private void OnEnable()
    {
        //Subscribe to everyting
        triggerDetector.AddActivatorTag(Tags.Player_Hitbox);
        triggerDetector.AddActivatorTag(Tags.Player);
        triggerDetector.OnTriggerEntered += triggerDetected;
        eventSystem.OnSpawned += SpawnedPush;
        eventSystem.OnBeingAttacked += AttackPush;
        eventSystem.OnBeingTouched += TouchedPush;
        eventSystem.OnHitWall += HitWallPush;
        
        //Make the deadparts ignore every other ground except their own:

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
        triggerDetector.OnTriggerExited -= triggerDetected;
        DeadParts_Manager.Instance.GroundsList.Remove(groundCollider);
        DeadParts_Manager.Instance.OnDeadPartInstantiated -= IgnoreOtherGrounds;
    }
    private void Start()
    {
        previusParentPosition = movingParent.position;
    }

    //Every Frame, check the last movement of the ground and replicate it in the simulated child
    private void Update()
    {
        //Turn OFF simulation
        DeadPart_RB.isKinematic = true;
        foreach (Rigidbody2D rb in ChildDeadParts) { rb.isKinematic = true; }

        //Get velocity and reset prev position
        Vector2 parentPosition = movingParent.position;
        Vector2 parentVelocity = parentPosition - previusParentPosition;
        previusParentPosition = parentPosition;
        
        //Add the velocity position to the non-simulated RB
        DeadPart_RB.position = DeadPart_RB.position + parentVelocity;

        //Trn ON simulation
        DeadPart_RB.isKinematic = false;
        foreach (Rigidbody2D rb in ChildDeadParts) { rb.isKinematic = false; }


        //Testing stuff
        if (Input.GetKey(KeyCode.E))
        {
            Vector2 normalizedDirection = debugDirection.normalized;
            if (!doVertical)
            {
                
                CallHorizontal(normalizedDirection, debugIntensity, 1f);
            }
            else
            {
                CallVertical(normalizedDirection, debugIntensity, 1f);
            }
        }
    }
    void triggerDetected(Collider2D collision)
    {
        //Find the direction
        Vector2 otherPosition = collision.gameObject.transform.root.position;
        Vector2 attackDirection = (DeadPart_RB.position - otherPosition).normalized;

        //Depending on what it touched, invoke a diferent event
        switch (collision.gameObject.tag)
        {
            case "Attack_Hitbox":
                eventSystem.OnBeingAttacked?.Invoke(this, new DeadPart_EventSystem_min.DeadPartArgs(attackDirection));
                break;
            case "Player":
                eventSystem.OnBeingTouched?.Invoke(this, new DeadPart_EventSystem_min.DeadPartArgs(attackDirection));
                break;
            default: break;
        }
    }
    void SpawnedPush(object sender, DeadPart_EventSystem_min.DeadPartArgs args)
    {
        CallVertical(args.Direction, 1f, 1f);
    }
    void AttackPush(object sender, DeadPart_EventSystem_min.DeadPartArgs args)
    {
        CallHorizontal(args.Direction, 1f, 1f);
    }
    void TouchedPush(object sender, DeadPart_EventSystem_min.DeadPartArgs args)
    {
        CallHorizontal(args.Direction, 0.2f, 0.5f);
    }
    void HitWallPush()
    {
        triggerDetector.GetComponent<Collider2D>().enabled = true;
        CallHorizontal(-currentDirection, 0.4f, 1f);
    }
    public void CallHorizontal(Vector2 direction, float intencity, float durationMultiplier)
    {
        triggerDetector.GetComponent<Collider2D>().enabled = false; //Disable triggerer so we dont have multiple inmediate pushes

        if (currentPush != null) { StopCoroutine(currentPush); } //Stop current push if it exists

        //Call the Push with the proper stats
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
        triggerDetector.GetComponent<Collider2D>().enabled = false;//Disable triggerer so we dont have multiple inmediate pushes

        if (currentPush != null) { StopCoroutine(currentPush); } //Stop current push if it exists

        //Call the Push with the proper stats
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
        //Wait a sec so everything instantiates right (onSpawnPush)
        yield return new WaitForSeconds(0.05f);

        float timer = 0;
        float randomTorque = UnityEngine.Random.Range(-maxRotation, maxRotation);
        Vector2 initialPosition = movingParent.position;

        triggerDetector.GetComponent<Collider2D>().enabled = true; 

        //Get a slightly random direction 
        Vector2 randomDirection = Quaternion.Euler(0, 0, Random.Range(random, -random)) * direction;
        currentDirection = randomDirection; //This is used OnHitWall so it pushes on the oposite direction

        while (timer < duration)
        {
            isPushed = true;
            timer += Time.deltaTime * Time.timeScale;

            
            float normalizedTime = Mathf.InverseLerp(0, duration, timer);
            float groundCurvePoint = groundAnimationCurve.Evaluate(normalizedTime);

            //Move the ground
            movingParent.position = initialPosition + (randomDirection * groundCurvePoint * intencity * distance);
            
           
            //At the beggining, add upward force and rotate the DeadPart
            if (timer < duration / 8)
            {
                verticalForce = Mathf.Lerp(verticalForce, 0, 0.1f); //Lerp smoothing the force
                DeadPart_RB.velocity = (Vector2.up * verticalForce * intencity * Time.timeScale);


                DeadPart_RB.AddTorque(randomTorque * Time.timeScale);

            }

            yield return null;
        }
        triggerDetector.GetComponent<Collider2D>().enabled = true;
        isPushed = false;
    }
    
    
}
