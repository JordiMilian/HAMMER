using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;

public class DeadPartTest : MonoBehaviour
{
    [SerializeField] Rigidbody2D Ground_RB;
    [SerializeField] Rigidbody2D DeadPart_RB;

    [SerializeField] Vector2 testDirection;

    [SerializeField] float DP_Force;

    [Range(0f, 1f)]
    [SerializeField] float groundAcceleration;
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

        //Get the ground and add it to the list of the manager
        groundCollider = Ground_RB.GetComponent<Collider2D>();
        DeadParts_Manager.Instance.GroundsList.Add(groundCollider);

        LayerMask DeadPartLayer = LayerMask.NameToLayer("DeadParts");
       // DP_Colliders = UsefullMethods.GetChildrensWithLayer(transform, DeadPartLayer);
        foreach (Collider2D col in DP_Colliders)
        {
            DeadParts_Manager.Instance.IgnoreAllGroundExceptThis(groundCollider, col);
        }
        
    }
    private void OnDisable()
    {
        attackTrigger.OnTriggerExited -= AttackDetected;
        DeadParts_Manager.Instance.GroundsList.Remove(groundCollider);

    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) 
        {
            StartCoroutine(PushMegaCoroutine(groundDuration, testDirection));
        }
    }
    private void FixedUpdate()
    {
       DeadPart_RB.position = new Vector2(Ground_RB.position.x, DeadPart_RB.position.y);
    }
    void AttackDetected(object sender, Generic_OnTriggerEnterEvents.EventArgsCollisionInfo args)
    {
        Debug.Log("Detected");
        Vector2 attackDirection = ( transform.position- args.Collision.gameObject.transform.root.position).normalized;
        StartCoroutine(PushMegaCoroutine(groundDuration, attackDirection));
    }
    IEnumerator PushMegaCoroutine(float duration, Vector2 direction)
    {
        float timer = 0;
        float lerpedStrenght = 1;

        while (timer < duration)
        {
            timer += Time.fixedDeltaTime;
            lerpedStrenght = Mathf.Lerp(lerpedStrenght, 0, groundAcceleration);
            Ground_RB.MovePosition(Ground_RB.position + (direction * lerpedStrenght * groundForce));
            if(timer < duration/2)
            {
                DeadPart_RB.AddForce((Vector2.up * DP_Force) + direction);
            }
            if(timer < duration/4)
            {
                DeadPart_RB.AddTorque(UnityEngine.Random.Range(-maxRandomRotation, maxRandomRotation));
            }
            yield return new WaitForFixedUpdate();
            Debug.Log("deathhh");
        }
    }
}
