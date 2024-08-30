using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class dialoguePole_animator : MonoBehaviour
{
    [SerializeField] Generic_EventSystem _eventSystem;
    [SerializeField] Animator animator;
    [SerializeField] VisualEffect breakEffect;
    private void OnEnable()
    {
        _eventSystem.OnReceiveDamage += setTrigger;
    }
    private void OnDisable()
    {
        _eventSystem.OnReceiveDamage -= setTrigger;
    }
    void setTrigger(object sender, Generic_EventSystem.ReceivedAttackInfo info)
    {
        animator.SetTrigger("hit");
        breakEffect.SetFloat("normalizedIntensity", 0.35f);
        breakEffect.Play();
    }

}
