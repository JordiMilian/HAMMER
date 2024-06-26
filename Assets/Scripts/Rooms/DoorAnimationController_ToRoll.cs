using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimationController_ToRoll : DoorAnimationController
{
    [SerializeField] Collider2D DamageCollider;
    private void OnEnable()
    {
        OnDoorClosed += HideDamageCollider;
        OnDoorOpened += ShowDamageColldier;
        OpenDoor();
        
    }
    void HideDamageCollider()
    {
        DamageCollider.enabled = false;
    }
    void ShowDamageColldier()
    {
        DamageCollider.enabled = true;
    }
}
