using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP_script : MonoBehaviour
{
    public int XpAmount;
    [SerializeField] Animator xpAnimator;

    private void OnEnable()
    {
        onSpawn();
    }
    private void OnDisable()
    {
        onPickedUp();
    }
    void onSpawn()
    {
        //xp spawned
    }
    void onPickedUp()
    {
        //xp picked up
    }
}
