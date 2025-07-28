using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Upgrades/HealOnLoadRoom", fileName = "HealOnLoadRoom")]
public class Upgrade_HealOnLoadRoom : Upgrade
{
    Player_References playerrefs;
    [SerializeField] AudioClip healSFX;
    [SerializeField] float HealPercent;
    public override void onAdded(GameObject entity)
    {
        
        playerrefs = entity.GetComponent<Player_References>();
        playerrefs.stateMachine.OnStateEntered += OnStateChanged;
        //GameEvents.OnLoadNewRoom += HealWithDelay;
    }
    void OnStateChanged(PlayerState newState)
    {
        if(newState is PlayerState_EnteringRoom)
        {
            CoroutinesRunner.instance.RunCoroutine(HealCoroutine());

            IEnumerator HealCoroutine()
            {
                yield return new WaitForSeconds(2f);
                playerrefs.flasher.CallDefaultFlasher();
                SFX_PlayerSingleton.Instance.playSFX(healSFX);
                IHealth playerHealth = playerrefs.GetComponent<IHealth>();
                float HPToAdd = playerHealth.GetMaxHealth() * UsefullMethods.normalizePercentage(HealPercent, false, false);

                playerHealth.RemoveHealth(-HPToAdd);
            }
        }
    }


    public override void onRemoved(GameObject entity)
    {
        playerrefs.stateMachine.OnStateEntered -= OnStateChanged;
    }

    public override string shortDescription()
    {
        return $"Heal {HealPercent}% on entering a new room";
    }

}
