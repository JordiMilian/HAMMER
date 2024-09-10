using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Animation : MonoBehaviour
{
    Generic_EventSystem buttonEvents;
    Animator buttonAnimator;
    private void OnEnable()
    {
        buttonEvents = GetComponent<Generic_EventSystem>();
        buttonAnimator = GetComponent<Animator>();
        buttonEvents.OnReceiveDamage += onGethit;
    }
    private void OnDisable()
    {
        buttonEvents.OnReceiveDamage -= onGethit;
    }
    void onGethit(object sender, Generic_EventSystem.ReceivedAttackInfo info)
    {
        buttonAnimator.SetTrigger("Hit");
    }
}
