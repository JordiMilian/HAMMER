
using System;
using System.Collections;

using UnityEngine;


//using UnityEngine.Windows;

public class Player_FeedbackManager : MonoBehaviour
{
    [SerializeField] Player_References playerRefs;
    [SerializeField] float staggerTime = 1;
    bool receivingDamage;

    

    private void OnEnable()
    {

        playerRefs.events.OnReceiveDamage += ReceiveDamageEffects;

        playerRefs.events.OnGettingParried += GettingParriedEffects;
        playerRefs.events.CallShowAndEnable += OnActivationFeedback;
        playerRefs.events.OnActuallySpecialHeal += HealFeedback;
        playerRefs.events.OnPickedNewUpgrade += PickUpUpgradeFeedback;
        playerRefs.events.OnPickedNewWeapon += PickUpWeaponFeedback;
    }
    private void OnDisable()
    {

        playerRefs.events.OnReceiveDamage -= ReceiveDamageEffects;

        playerRefs.events.OnGettingParried -= GettingParriedEffects;
        playerRefs.events.CallShowAndEnable -= OnActivationFeedback;
        playerRefs.events.OnActuallySpecialHeal -= HealFeedback;
        playerRefs.events.OnPickedNewUpgrade -= PickUpUpgradeFeedback;
        playerRefs.events.OnPickedNewWeapon -= PickUpWeaponFeedback;
    }

    public void ReceiveDamageEffects(object sender, Player_EventSystem.ReceivedAttackInfo receivedAttackinfo)
    {
        if(!receivingDamage)
        {
            receivingDamage = true;
            StartCoroutine(InvulnerableAfterDamage());
        }
    }
    void GettingParriedEffects(Generic_EventSystem.GettingParriedInfo info)
    {
        Player_ComboSystem_chargeless comboSystem = GetComponent<Player_ComboSystem_chargeless>();
        comboSystem.EV_HideWeaponCollider();
        playerRefs.playerMovement.EV_ReturnSpeed();
        playerRefs.animator.SetTrigger("Parried");
    }
     IEnumerator InvulnerableAfterDamage()
    {
        yield return new WaitForSeconds(staggerTime);
        playerRefs.currentStats.Speed = playerRefs.currentStats.BaseSpeed;
        receivingDamage = false;
    }
    void OnActivationFeedback()
    {
        playerRefs.animator.SetTrigger("Reactivated");
        CameraShake.Instance.ShakeCamera(0.5f, 0.3f);
    }
    void HealFeedback()
    {
        CameraShake.Instance.ShakeCamera(.2f, 0.1f);
        playerRefs.flasher.CallDefaultFlasher();
    }
    void PickUpUpgradeFeedback(UpgradeContainer upgrade)
    {
        CameraShake.Instance.ShakeCamera(.3f, 0.1f);
        TimeScaleEditor.Instance.HitStop(0.03f);
        playerRefs.flasher.CallDefaultFlasher();
    }
    void PickUpWeaponFeedback(WeaponPrefab_infoHolder info)
    {
        CameraShake.Instance.ShakeCamera(.3f, 0.1f);
        TimeScaleEditor.Instance.HitStop(0.05f);
        playerRefs.flasher.CallDefaultFlasher();
    }
}

   
