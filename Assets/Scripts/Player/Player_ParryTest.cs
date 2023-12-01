using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ParryTest : MonoBehaviour
{
   [SerializeField] Collider2D ParryCollider;
   [SerializeField] Collider2D PlayerDamageCollider;
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(1)) 
        {
            animator.SetTrigger("Parry");
        }
    }
    public void EV_ShowParryCollider()
    {
        ParryCollider.enabled = true;
        PlayerDamageCollider.enabled = false;
    }
    public void EV_HideParryColldier()
    {
        ParryCollider.enabled = false;
        PlayerDamageCollider.enabled = true;
    }
}
