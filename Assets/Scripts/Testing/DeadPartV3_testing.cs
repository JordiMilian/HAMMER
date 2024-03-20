using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeadPartV3_testing : MonoBehaviour
{
    [SerializeField] Rigidbody2D DeadPart_RB;
    [SerializeField] Transform movingParent;
    [SerializeField] Collider2D groundCollider;
    [SerializeField] Generic_OnTriggerEnterEvents triggerDetector;
    

    [Header("Push Stats")]
    [SerializeField] float pushDuration;
    [SerializeField] float maxRandomRotation;
    [SerializeField] float groundAddedForce = 0.1f;
    [SerializeField] float deadPartAddedForce = 1;
    [Range(0, 1)]
    [SerializeField] float groundAcceleration = 0.1f;

    List<Collider2D> DeadPart_relatedColliders = new List<Collider2D>();
    Vector2 previusParentPosition;
    Coroutine currentPush;

    private void OnEnable()
    {
        triggerDetector.AddActivatorTag(TagsCollection.Instance.Attack_Hitbox);
        triggerDetector.AddActivatorTag(TagsCollection.Instance.Player);
        triggerDetector.OnTriggerEntered += AttackDetected;

        

        //Get the ground and add it to the list of the manager

        DeadParts_Manager.Instance.GroundsList.Add(groundCollider);

        //FInd all the objects with DeadPart Layer and add their collider to a List
        LayerMask DeadPartLayer = LayerMask.NameToLayer("DeadParts");
        GameObject[] DP_GO = UsefullMethods.GetChildrensWithLayer(transform, DeadPartLayer);
        foreach (GameObject go in DP_GO)
        {
            if (go.GetComponent<Collider2D>() != null)
            {
                DeadPart_relatedColliders.Add(go.GetComponent<Collider2D>());
            }
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
        triggerDetector.OnTriggerExited -= AttackDetected;
        DeadParts_Manager.Instance.GroundsList.Remove(groundCollider);
        DeadParts_Manager.Instance.OnDeadPartInstantiated -= IgnoreOtherGrounds;
    }
    private void Start()
    {
        previusParentPosition = movingParent.position;
    }
    private void Update()
    {
        //Turn OFF simulation
        DeadPart_RB.isKinematic = true;

        //Get velocity and reset prev position
        Vector2 parentPosition = movingParent.position;
        Vector2 parentVelocity = parentPosition - previusParentPosition;
        previusParentPosition = parentPosition;
        
        //Add the velocity position to the non-simulated RB
        DeadPart_RB.position = DeadPart_RB.position + parentVelocity;

        //Trn ON simulation
        DeadPart_RB.isKinematic = false;


        //Whatever else
        if(Input.GetKey(KeyCode.E))
        {
            DeadPart_RB.AddForce(Vector2.up * 50);
            DeadPart_RB.AddTorque(10);
        }
    }
    void AttackDetected(object sender, Generic_OnTriggerEnterEvents.EventArgsCollisionInfo args)
    {
        Debug.Log("attack detected");
        Vector2 otherPosition = args.Collision.gameObject.transform.position;
        Vector2 attackDirection = (DeadPart_RB.position - otherPosition).normalized;
        switch (args.Collision.gameObject.tag)
        {
            case "Attack_Hitbox":
                currentPush = StartCoroutine(PushCoroutine(attackDirection, 1f));
                break;
            case "Player":
                currentPush = StartCoroutine(PushCoroutine(attackDirection, 0.2f));
                break;
            default: break;
        }
    }
    public IEnumerator PushCoroutine( Vector2 direction, float intencity)
    {
        float timer = 0;
        float lerpedStrenght = 1;
        float randomTorque = UnityEngine.Random.Range(-maxRandomRotation, maxRandomRotation);

        triggerDetector.GetComponent<Collider2D>().enabled = false;
        while (timer < pushDuration)
        {
            timer += Time.fixedDeltaTime;

            //Ground decelerator and move the ground
            lerpedStrenght = Mathf.Lerp(lerpedStrenght, 0, groundAcceleration);
            movingParent.Translate(direction * lerpedStrenght * groundAddedForce * intencity);
            Debug.Log(direction + " - " + lerpedStrenght + " - " +groundAddedForce + " - " +intencity);
            
            //For the first half of time, push Deadport upwards
           
            //For the first quarter, rotate the DeadPart
            if (timer < pushDuration / 4)
            {
                DeadPart_RB.AddForce(Vector2.up * deadPartAddedForce * intencity);
                DeadPart_RB.AddTorque(randomTorque);
            }

            //FIxed Update
            yield return null;
        }

        triggerDetector.GetComponent<Collider2D>().enabled = true;
    }
}
