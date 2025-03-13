
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

        playerRefs.events.CallShowAndEnable += OnActivationFeedback;
        playerRefs.events.OnActuallySpecialHeal += HealFeedback;
        playerRefs.events.OnPickedNewUpgrade += PickUpUpgradeFeedback;
        playerRefs.events.OnPickedNewWeapon += PickUpWeaponFeedback;
    }
    private void OnDisable()
    {
        playerRefs.events.CallShowAndEnable -= OnActivationFeedback;
        playerRefs.events.OnActuallySpecialHeal -= HealFeedback;
        playerRefs.events.OnPickedNewUpgrade -= PickUpUpgradeFeedback;
        playerRefs.events.OnPickedNewWeapon -= PickUpWeaponFeedback;
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

   
