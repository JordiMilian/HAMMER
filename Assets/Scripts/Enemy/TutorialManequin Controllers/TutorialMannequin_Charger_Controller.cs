using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMannequin_Charger_Controller : MonoBehaviour, IDamageReceiver
{
    [SerializeField] Animator animator;
    [SerializeField] Generic_Flash flasher;

    public Action<ReceivedAttackInfo> OnDamageReceived_event { get; set; }
    public void OnDamageReceived(ReceivedAttackInfo info)
    {
        animator.SetTrigger("Hit");
        flasher.CallDefaultFlasher();
        Debug.Log("Add damage??");
        if(info.OtherDamageDealer.rootGameObject_DamageDealerTf.TryGetComponent(out Player_Controller player))
        {
            player.addSpecialCharge(2);
            Debug.Log("Added damg");
        }

        OnDamageReceived_event?.Invoke(info);
    }
}
