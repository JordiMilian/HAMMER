using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadParts_Manager : MonoBehaviour
{
    public List<Collider2D> GroundsList = new List<Collider2D>();

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
        foreach (Collider2D col in GroundsList)
        {
            if (col == ownGround) { continue; }
            Physics2D.IgnoreCollision(ownGround, DeadPartCollider);
        }
    }
}
