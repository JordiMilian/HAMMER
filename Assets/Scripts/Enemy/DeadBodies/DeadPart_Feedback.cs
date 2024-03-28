using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DeadPart_Feedback : MonoBehaviour
{
    [SerializeField] DeadPart_EventSystem eventSystem;
    [SerializeField] Transform Ground_TF;
    [SerializeField] Animator deadPart_Animator;
    [SerializeField] VisualEffect BloodTrail;
    [SerializeField] Generic_Flash flasher;
    float bloodSplashIntensity;

    private void OnEnable()
    {
        eventSystem.OnSpawned += spawnedFeedback;
        eventSystem.OnReceiveDamage += GettingHitFeedback;
        eventSystem.OnBeingTouchedObject += GettingTouchedFeedback;
        eventSystem.OnHitWall += HittingWallFeedback;
    }
    void spawnedFeedback(object sender, Generic_EventSystem.ObjectDirectionArgs args)
    {
        Debug.Log("spawn feedbaked");
        StartCoroutine(BloodStopper());
        deadPart_Animator.SetTrigger("Light");
        flasher.CallFlasher();
    }
    void GettingHitFeedback(object sender, Generic_EventSystem.ReceivedAttackInfo args)
    {
        TimeScaleEditor.Instance.HitStop(0.05f);
        CameraShake.Instance.ShakeCamera(0.5f, 0.1f);
        GroundBloodPlayer.Instance.PlayGroundBlood(Ground_TF.position, args.GeneralDirection, bloodSplashIntensity);
        deadPart_Animator.SetTrigger("Strong");
    }
    void GettingTouchedFeedback(object sender, Generic_EventSystem.ObjectDirectionArgs args)
    {
        deadPart_Animator.SetTrigger("Light");
    }
    void HittingWallFeedback()
    {
        deadPart_Animator.SetTrigger("Light");
    }
    IEnumerator BloodStopper()
    {
        bloodSplashIntensity = 0.75f;
        float timer = 0;
        while (timer < 7f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        BloodTrail.Stop();
        bloodSplashIntensity = 0.4f;
    }
}
