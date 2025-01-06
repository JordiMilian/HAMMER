using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AgrooAlertIcon : MonoBehaviour
{
    [SerializeField] Enemy_References enemyRefs;

    private void Awake()
    {
        enemyRefs.enemyEvents.OnPlayerDetected += playAlertIcon; 
    }
    void playAlertIcon()
    {
        Animator iconAnimator = GetComponent<Animator>();
        iconAnimator.SetTrigger("AgrooAlert");
    }
}
