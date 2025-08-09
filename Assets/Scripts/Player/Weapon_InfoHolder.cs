using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_InfoHolder : MonoBehaviour
{
    public PolygonCollider2D WeaponCollider;
    public Sprite WeaponSprite;

    public PlayerState SpecialAttack, RollAttack, ParryAttack, RunningAttack;
    public List<PlayerState> ComboAttacksList; //Index 0 should be the starting combo attack
}
