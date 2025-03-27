using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pathfinding.Util.RetainedGizmos;

public class TutorialMannequin_NoAttacker_Controller : MonoBehaviour, IDamageReceiver
{
    [SerializeField] Animator animator;
    [SerializeField] Generic_Flash flasher;
     
    public Action<ReceivedAttackInfo> OnDamageReceived_event { get; set; }
    public void OnDamageReceived(ReceivedAttackInfo info)
    {
        animator.SetTrigger("Hit");
        flasher.CallDefaultFlasher();

        OnDamageReceived_event?.Invoke(info);
    }

}
