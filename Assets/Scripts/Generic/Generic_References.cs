using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_References : MonoBehaviour
{
    [Header("Generic")]
    public Generic_EventSystem genericEvents;
    public Generic_DamageDealer damageDealer;
    public Generic_DamageDetector damageDetector;
    public Generic_Stats stats;
    public Generic_FlipSpriteWithFocus spriteFliper;
    public Generic_Flash flasher;
    public Animator animator;
    public Collider2D weaponCollider;
    public Collider2D damageDetectorCollider;
    public Collider2D positionCollider;
    public Rigidbody2D _rigidbody;
}
