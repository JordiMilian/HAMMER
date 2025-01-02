using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AgrooAlertIcon : MonoBehaviour
{
    [SerializeField] Enemy_References enemyRefs;

    private void Awake()
    {
        enemyRefs.enemyEvents.OnExitIdle += playAlertIcon; //Puede que de problemas por si hacemos transiciones de Idle?
    }
    void playAlertIcon()
    {
        Animator iconAnimator = GetComponent<Animator>();
        iconAnimator.SetTrigger("AgrooAlert");

    }
}
