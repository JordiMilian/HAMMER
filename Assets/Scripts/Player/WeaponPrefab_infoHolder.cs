using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPrefab_infoHolder : MonoBehaviour
{
    public PolygonCollider2D weaponsCollider;
    public Sprite weaponSprite;
    public float BaseDamage;
    public float BaseKnockback;
    public float BaseHitstop;
    public float StaminaUsePerSwing;
    [Header("Special Attack")]
    public float Sp_Damage;
    public float Sp_Knockback;
    public float Sp_StaminaCost;
    public RuntimeAnimatorController animatorController;
    [SerializeField] Animator animator;
    public Action<WeaponPrefab_infoHolder> OnPickedUpEvent;
    public int indexInGameState;

    private void OnEnable()
    {
        OnSpawned();
    }
    public void OnSpawned()
    {

    }
    public void OnPickedUp()
    {
        animator.SetTrigger("PickedUp");
        StartCoroutine( UsefullMethods.destroyWithDelay(.6f,gameObject));
        OnPickedUpEvent?.Invoke(this);
    }

}
