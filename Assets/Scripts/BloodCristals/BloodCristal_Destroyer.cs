using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCristal_Destroyer : MonoBehaviour
{
    [SerializeField] GameObject Parent;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Attack_Hitbox"))
        {
            Debug.Log("Blood");
            Destroy(Parent);
        }
    }

}
