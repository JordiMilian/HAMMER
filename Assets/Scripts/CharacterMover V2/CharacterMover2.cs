using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover2 : MonoBehaviour
{
    public Vector2 currentVelocity;
    public List<Vector2> MovementVectorsPerSecond = new List<Vector2>();
    public CircleCollider2D circleCollider;
    [Header("Testing")]
    [SerializeField] Vector2 TestDirectionToMove;
    [SerializeField] float speedMultiplier = 0.01f;

    private void Start()
    {
        CollisionsManager.instance.AddCharacterMover(this);
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

        currentVelocity = calculatedDirection;

    }
}
