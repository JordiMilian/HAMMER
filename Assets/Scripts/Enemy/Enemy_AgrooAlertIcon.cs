using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AgrooAlertIcon : MonoBehaviour
{

    public void playAlertIcon()
    {
        Animator iconAnimator = GetComponent<Animator>();
        iconAnimator.SetTrigger("AgrooAlert");
    }
}
