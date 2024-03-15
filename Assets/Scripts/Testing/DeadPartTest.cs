using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPartTest : MonoBehaviour
{
    [SerializeField] Rigidbody2D GroundRB;
    [SerializeField] Rigidbody2D CurrentRB;
    private void FixedUpdate()
    {
        Vector2 currentPos = CurrentRB.position;

        CurrentRB.position = new Vector2(GroundRB.position.x, currentPos.y);
    }
}
