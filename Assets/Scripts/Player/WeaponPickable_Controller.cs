using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickable_Controller : MonoBehaviour
{
    CircleCollider2D detectionCollider;
    public Action<WeaponPickable_Controller> OnPickedUpEvent;
    Animator animator;
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        OnSpawned();
        detectionCollider = GetComponent<CircleCollider2D>();
    }
    public void OnSpawned()
    {

    }
    public void OnPickedUp()
    {
        animator.SetTrigger("PickedUp");
        detectionCollider.enabled = false;
        StartCoroutine(UsefullMethods.destroyWithDelay(.6f, gameObject));
        OnPickedUpEvent?.Invoke(this);
    }
}
