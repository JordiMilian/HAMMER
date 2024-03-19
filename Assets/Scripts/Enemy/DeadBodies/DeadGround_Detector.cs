using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadGround_Detector : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Detected some " + collision.gameObject.layer.ToString());
        if (collision.gameObject.layer == LayerMask.NameToLayer("BlockingWalls"))
        {
            Debug.Log("I SHOULD STOP");
        }
    }
}
