using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkSawProjectile_Controller : MonoBehaviour, IDamageDealer, IParryReceiver
{
    [SerializeField] Animator sawAnimator;
    [SerializeField] Transform sawTf;
    [SerializeField] float timeToReach;
    [SerializeField] float distance;
    public Vector2 InitialPos, FinalPos;
    [SerializeField] SpriteMask spriteMask;
    [SerializeField] SpriteRenderer sawSprite;
    [SerializeField] PinkSaw_OrientationSetter_Projectile orientationSetter;
    [SerializeField] Collider2D damageCollider;

   

    public void startSawing(Vector2 initialPos, Vector2 directionToTarget)
    {
        InitialPos = initialPos;
        FinalPos = initialPos + (directionToTarget*distance);
        transform.position = InitialPos;
        
        sawAnimator.SetBool("isSawing", true);

        orientationSetter.setOritentation();
    }
    public void EV_StartMoving()
    {
        StartCoroutine(travelToPos(InitialPos, FinalPos, timeToReach));
    }
    IEnumerator travelToPos(Vector2 initialPos, Vector2 finalPos, float timeToReach)
    {
        float timer = 0;
        float normalizedTime = 0;
        while (timer < timeToReach)
        {
            timer += Time.deltaTime;
            normalizedTime = timer / timeToReach;

            Vector2 newPos = Vector2.Lerp(initialPos, finalPos, BezierBlend(normalizedTime));

            sawTf.position = newPos;
            yield return null;
        }
        reachedEnd();
    }
    void reachedEnd()
    {
        sawAnimator.SetTrigger("reachedEnd");
    }
    public void EV_SawIsHidden()
    {
        Destroy(gameObject);
    }
    public void EV_ShowDamageCollider()
    {
        damageCollider.enabled = true;
    }
    public void EV_HideDamageCollider()
    {
        damageCollider.enabled = false;
    }

    float BezierBlend(float t)
    {
        return t * t * (3.0f - 2.0f * t);
    }
    public Action<DealtDamageInfo> OnDamageDealt_event { get; set; }
    public Action<GettingParriedInfo> OnParryReceived_event { get; set; }

    public void OnDamageDealt(DealtDamageInfo info)
    {
        OnDamageDealt_event?.Invoke(info);
    }

    public void OnParryReceived(GettingParriedInfo info)
    {
        OnParryReceived_event?.Invoke(info);
        EV_HideDamageCollider();
        //TO DO: Stop moving
    }
}
