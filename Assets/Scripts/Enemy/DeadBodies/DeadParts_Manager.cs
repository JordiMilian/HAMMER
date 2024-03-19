using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadParts_Manager : MonoBehaviour
{
    public List<Collider2D> GroundsList = new List<Collider2D>();
    public Action OnDeadPartInstantiated;

    public static DeadParts_Manager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void IgnoreAllGroundExceptThis(Collider2D ownGround, Collider2D DeadPartCollider)
    {
        int equals = 0;
        int diferents = 0;
        foreach (Collider2D groundCol in GroundsList)
        {
            if (groundCol == ownGround) { equals++; continue; }
            diferents++;
            Physics2D.IgnoreCollision(groundCol, DeadPartCollider);
        }
        Debug.Log("Equals: " +  equals + "  Diferents: " + diferents);
    }
}
