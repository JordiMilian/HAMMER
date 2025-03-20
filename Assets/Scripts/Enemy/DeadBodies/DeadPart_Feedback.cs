using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DeadPart_Feedback : MonoBehaviour
{
    //THIS ALL SHOULD PROBABLY BE INSIDE CONTROLLER?

    [SerializeField] DeadPart_EventSystem eventSystem;
    [SerializeField] Transform Ground_TF;
    [SerializeField] Animator deadPart_Animator;
    [SerializeField] VisualEffect BloodTrail;
    [SerializeField] Generic_Flash flasher;
    [SerializeField] GameObject spritesRoot;
    [SerializeField] SpriteRenderer shadowSprite;
    [SerializeField] bool dontDestroyOnRespawn;
    float bloodSplashIntensity;
    [SerializeField] float secondsToFadeOut = 8;

    private void OnEnable()
    {
        
        
        eventSystem.OnHitWall += HittingWallFeedback;
        GameEvents.OnPlayerRespawned += DestroyOnRespawn;
    }
    private void OnDisable()
    {
        GameEvents.OnPlayerRespawned -= DestroyOnRespawn;
    }
    void spawnedFeedback()
    {
        StartCoroutine(BloodStopper());
        deadPart_Animator.SetTrigger("Light");
        flasher.CallDefaultFlasher();
    }
    /*
    void GettingHitFeedback(object sender, Generic_EventSystem.ReceivedAttackInfo args)
    {
        TimeScaleEditor.Instance.HitStop(0.05f);
        CameraShake.Instance.ShakeCamera(0.5f, 0.1f);
        GroundBloodPlayer.Instance.PlayGroundBlood(Ground_TF.position, args.GeneralDirection, bloodSplashIntensity);
        deadPart_Animator.SetTrigger("Strong");
    }
    */
    void GettingTouchedFeedback()
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
    //Fade out sprites and destroy after a while
    /*
    private IEnumerator Start()
    {
        SpriteRenderer[] spritesArray = spritesRoot.GetComponentsInChildren<SpriteRenderer>();
        yield return new WaitForSeconds(secondsToFadeOut);
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime;
            Color fadeColor = new Color(1, 1, 1, 1 - timer);
            foreach(SpriteRenderer sprite in spritesArray)
            {
                sprite.color = fadeColor;
            }
            shadowSprite.color = fadeColor * Color.black;
            yield return null;
        }
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
    */
    void DestroyOnRespawn()
    {
        if (dontDestroyOnRespawn) { return; }
        Destroy(gameObject);   
    }
}
