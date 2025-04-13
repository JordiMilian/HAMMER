using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_References : MonoBehaviour
{
    [Header("Generic")]
    public List<Generic_DamageDealer> DamageDealersList;
    public Generic_DamageDetector damageDetector;
    public Generic_FlipSpriteWithFocus spriteFliper;
    public Generic_Flash flasher;
    public Animator animator;
    public Collider2D damageDetectorCollider;
    public Collider2D positionCollider;
    public CharacterMover2 characterMover;
}
