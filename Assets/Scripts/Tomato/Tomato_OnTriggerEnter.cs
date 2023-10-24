using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato_OnTriggerEnter : MonoBehaviour
{
    Tomato_Controller controller;
    private void Start()
    {
        controller = GetComponentInParent<Tomato_Controller>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            controller.DestroyTomato();
        }
        if (collision.tag == "Attack_Hitbox") 
        {
            controller.DestroyTomato();
        }
    }
}
