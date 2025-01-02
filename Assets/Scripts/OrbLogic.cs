using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbLogic : MonoBehaviour
{

    [SerializeField] Generic_OnTriggerEnterEvents orbTrigger;
    Collider2D orbCollider;
    SpriteRenderer spriteRend;
    [HideInInspector] public bool isCollected;
    public Action OnPickedUp;
    public enum OrbType
    {
        HealthOrb,SpeedOrb,DamageOrb,
    }
    [SerializeField] OrbType ThisType;
    private void Awake()
    {
        orbCollider = orbTrigger.GetComponent<Collider2D>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        orbTrigger.AddActivatorTag(Tags.Player_Hitbox);
        orbTrigger.OnTriggerEntered += PickedUp;
    }
    private void OnDisable()
    {
        orbTrigger.OnTriggerEntered -= PickedUp;
    }

    private void Start()
    {
        orbCollider.enabled = false;
        spriteRend.enabled = false;
    }
    public void Spawn()
    {
        orbCollider.enabled = true;
        spriteRend.enabled = true;
    }
    public void PickedUp(Collider2D collision)
    {
        orbCollider.enabled = false;

        isCollected = true;
        spriteRend.color = Color.yellow;
        OnPickedUp?.Invoke();
    }
    public void Despawn()
    {
        orbCollider.enabled = false;
        spriteRend.enabled = false;
    }
}
