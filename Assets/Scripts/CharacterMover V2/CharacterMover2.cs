using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover2 : MonoBehaviour
{
    public Vector2 currentVelocity; 
    public List<Vector2> MovementVectorsPerSecond = new List<Vector2>();
    public CircleCollider2D circleCollider;
    [Range(0f, 1f)]
    [Tooltip("Less Value means more Resistance/Strenght")]
    public float InvertedResistance = 0.5f; 
    //Value 0 means this object will not get moved by nobody. Use for unnmovable objects, such as respawn points
    //Value 1 means this object has little power to influence movement into others when colliding
    [Header("Root Motion")]
    [SerializeField] Animator animator;
    [SerializeField] float RootMotionMultiplier = 1;
    [Header("Testing")]
    [SerializeField] Vector2 TestDirectionToMove;
    [SerializeField] float speedMultiplier = 0.01f;
  
    private void Start()
    {
        CollisionsManager.instance.AddCharacterMover(this);
    }
    private void OnDisable()
    {
        CollisionsManager.instance.RemoveCharacterMover(this);
    }
    private void OnAnimatorMove()
    {
        return; //I believe this overrunes the default rootmotions
    }
    private void Update()
    {
        MovementVectorsPerSecond.Add(TestDirectionToMove * speedMultiplier); //For testing delete
        Vector2 calculatedDirection = Vector2.zero;

        // ----  MOVEMENTS  ----

        foreach (Vector2 movement in MovementVectorsPerSecond)
        {
            calculatedDirection += movement * Time.deltaTime;
        }
        MovementVectorsPerSecond.Clear();

        //    --  ROOT MOTION --
        if (animator != null)
        {
            calculatedDirection += (Vector2)animator.deltaPosition * RootMotionMultiplier;
        }

        //Limit the movement to the radius
        if (calculatedDirection.magnitude > circleCollider.radius)
        {
            calculatedDirection = calculatedDirection.normalized * circleCollider.radius;
            Debug.LogWarning("Movement had to be limited");
        }

        currentVelocity = calculatedDirection;

    }
}
