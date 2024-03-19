using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class DeadPartTest : MonoBehaviour
{
    [SerializeField] Rigidbody2D Ground_RB;
    [SerializeField] Rigidbody2D DeadPart_RB;

    [SerializeField] Vector2 testDirection;

    [SerializeField] float DP_Force;
    [Range(0, 1)]
    [SerializeField] float Intensity_tester;

    //[Range(0f, 1f)]
    //[SerializeField] float groundAcceleration;
    [SerializeField] float groundForce;
    [SerializeField] float groundDuration;
    [SerializeField] float maxRandomRotation;

    [SerializeField] Generic_OnTriggerEnterEvents attackTrigger;
    Collider2D groundCollider;
    List<Collider2D> DP_Colliders = new List<Collider2D>();

    private void OnEnable()
    {
        attackTrigger.AddActivatorTag(TagsCollection.Instance.Attack_Hitbox);
        attackTrigger.AddActivatorTag(TagsCollection.Instance.Player);
        attackTrigger.AddActivatorTag(TagsCollection.Instance.Player_SinglePointCollider);
        attackTrigger.OnTriggerEntered += AttackDetected;
        DeadParts_Manager.Instance.OnDeadPartInstantiated += CheckIgnorance;

        //Get the ground and add it to the list of the manager
        groundCollider = Ground_RB.GetComponent<Collider2D>();
        DeadParts_Manager.Instance.GroundsList.Add(groundCollider);

        //FInd all the objects with DeadPart Layer and add their collider to a List
        LayerMask DeadPartLayer = LayerMask.NameToLayer("DeadParts");
        GameObject[] DP_GO = UsefullMethods.GetChildrensWithLayer(transform, DeadPartLayer);
        foreach (GameObject go in DP_GO)
        {
            if(go.GetComponent<Collider2D>() != null)
            {
                DP_Colliders.Add(go.GetComponent<Collider2D>());
            }
        }

        //Invoke the Event that calls everyone to revisit what colliders to ignore 
        DeadParts_Manager.Instance.OnDeadPartInstantiated?.Invoke();
    }
    void CheckIgnorance()
    {
        //Make every Collider listed to Ignore every Ground except its own
        foreach (Collider2D col in DP_Colliders)
        {
            DeadParts_Manager.Instance.IgnoreAllGroundExceptThis(groundCollider, col);
        }
    }

    private void OnDisable()
    {
        attackTrigger.OnTriggerExited -= AttackDetected;
        DeadParts_Manager.Instance.GroundsList.Remove(groundCollider);
        DeadParts_Manager.Instance.OnDeadPartInstantiated -= CheckIgnorance;
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) 
        {
            StartCoroutine(PushMegaCoroutine(testDirection,Intensity_tester));
        }
        /*
        GameObject player = GameObject.Find(TagsCollection.Instance.MainCharacter);
        Vector3 attackDirection = (transform.root.position - player.transform.root.position).normalized;
        Debug.DrawLine(transform.root.position, transform.root.position + attackDirection);
        */
    }
    private void FixedUpdate()
    {
       DeadPart_RB.position = new Vector2(Ground_RB.position.x, DeadPart_RB.position.y);
    }
    void AttackDetected(object sender, Generic_OnTriggerEnterEvents.EventArgsCollisionInfo args)
    {
        Vector2 otherPosition = args.Collision.gameObject.transform.root.position;
        Vector2 attackDirection = ( DeadPart_RB.position - otherPosition).normalized;
        switch (args.Collision.gameObject.tag)
        {
            case "Attack_Hitbox":
                StartCoroutine(PushMegaCoroutine(attackDirection, 0.5f));
                break;
            case "Player":
                StartCoroutine(PushMegaCoroutine(attackDirection, 0.2f));
                break;
            default: break;
        }
            
        
    }
    public IEnumerator PushMegaCoroutine( Vector2 direction, float intencity)
    {
        float timer = 0;
        float lerpedStrenght = 1;
        float randomTorque = UnityEngine.Random.Range(-maxRandomRotation, maxRandomRotation);

        while (timer < groundDuration)
        {
            timer += Time.fixedDeltaTime;
            attackTrigger.enabled = false;
            //Ground decelerator and move the ground
            lerpedStrenght = Mathf.Lerp(lerpedStrenght, 0, 0.25f);
            Ground_RB.MovePosition(Ground_RB.position + (direction * lerpedStrenght * groundForce*intencity));
            
            //For the first half of time, push Deadport upwards
            if(timer < groundDuration/ 3)
            {
                DeadPart_RB.AddForce((Vector2.up * DP_Force * intencity) + direction);
            }

            //For the first quarter, rotate the DeadPart
            if (timer < groundDuration/ 4)
            {
                DeadPart_RB.AddTorque(randomTorque);
            }


            //FIxed Update
            yield return new WaitForFixedUpdate();
        }
        attackTrigger.enabled = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Detected some");
        if(collision.gameObject.layer == LayerMask.NameToLayer("BlockingWalls"))
        {
            Debug.Log("I SHOULD STOP");
        }
    }
}
