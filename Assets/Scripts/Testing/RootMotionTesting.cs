using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RootMotionTesting : MonoBehaviour
{
    public List<Vector2> MovementVectors = new List<Vector2>(); //Any movement stuff must be added here everyframe
    public float RootMotionMultiplier = 1; //This could be useful to adapt attacks to distance of player

    [SerializeField] Animator animator;

    List<Vector2> collisionDirections = new List<Vector2>(); //For DrawGizmo. Delete at some point
    List<Collider2D> collidersInside = new List<Collider2D>();
    
    
    [Header("Testing")]
    [SerializeField] Vector2 TestDirectionToMove;
    [SerializeField] float speedMultiplier = 0.01f;
    
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        MovementVectors.Add(TestDirectionToMove);
        MovementVectors.Add(Vector2.zero);
    }
    private void Update()
    {
        collisionDirections.Clear();

        Vector2 calculatedDirection = Vector2.zero;

        MovementVectors.Add(TestDirectionToMove * speedMultiplier); //For testing delete

        //MOVEMENTS
        foreach(Vector2 movement in MovementVectors)
        {
            calculatedDirection += movement * Time.deltaTime;
        }
        MovementVectors.Clear(); //Not sure if I should clear this here

        //ROOT MOTION
        calculatedDirection += (Vector2)animator.deltaPosition * RootMotionMultiplier;

        //COLISIONS 
        //Afegim la mateix força aplicada en la direccio contraria a la pared
        foreach (var collider in collidersInside)
        {
            Vector2 closetsPoint = collider.ClosestPoint(transform.position);
            Vector2 direction = (closetsPoint - (Vector2)transform.position).normalized;
            collisionDirections.Add(direction); //This is for draw gizmos. Can be deleted
            calculatedDirection += (-direction * calculatedDirection.magnitude);
        }

        transform.Translate(calculatedDirection);
    }
    private void OnTriggerEnter2D(Collider2D collision) //The Layers set-up must be perfect
    {
        collidersInside.Add(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        collidersInside.Remove(collision);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach(Vector2 direction in collisionDirections)
        {
            Gizmos.DrawWireSphere((Vector2)transform.position + direction, 0.1f);
        }
        
    }
}
