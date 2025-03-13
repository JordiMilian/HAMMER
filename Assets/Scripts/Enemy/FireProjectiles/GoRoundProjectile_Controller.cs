using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoRoundProjectile_Controller : MonoBehaviour , IDamageDealer, IDamageReceiver, IParryReceiver
{
    [SerializeField] Transform RotatingPos;
    [SerializeField] Transform RotationRoot;
    [SerializeField] Transform ProjectileTf;
    public float rotationRadius;
    [SerializeField] float timeToReachFinalRadius;
    [SerializeField] float rotationSpeed_DegPerSec;

    [SerializeField] float timeToSelfDestroy;

    public Action<DealtDamageInfo> OnDamageDealt_event { get; set; }
    public void OnDamageDealt(DealtDamageInfo info)
    {
        destroyItself();
        OnDamageDealt_event?.Invoke(info);
    }
    public Action<ReceivedAttackInfo> OnDamageReceived_Event { get; set; }
    public void OnDamageReceived(ReceivedAttackInfo info)
    {
        BounceAway(info.RootsDirection);
        OnDamageReceived_Event?.Invoke(info);
    }
    public Action<GettingParriedInfo> OnParryReceived_event { get; set; }
    public void OnParryReceived(GettingParriedInfo info)
    {
        BounceAway(info.ParryDirection);
        OnParryReceived_event?.Invoke(info);
    }
    private void Awake()
    {
        if (currentMovement != null) { StopCoroutine(currentMovement); }
        currentMovement = StartCoroutine(followRotationCoroutine());
        StartCoroutine(selfDestroyInSeconds(timeToSelfDestroy));
    }
    [Header("Bounce")]
    [SerializeField] float bounceDuration;
    [SerializeField] float bounceInitialSpeed;
    [SerializeField] Generic_DamageDealer damageDealer;
    [SerializeField] Generic_DamageDetector damageDetector;
    public void SetStartingRotation(float startingRotationDeg)
    {
        RotationRoot.eulerAngles = new Vector3(0, 0, startingRotationDeg);
    }
     void BounceAway(Vector2 directionAway)
    {
        if (currentMovement != null) { StopCoroutine(currentMovement);}; 

        StartCoroutine(BounceAwayCoroutine(directionAway));

        damageDealer.EntityTeam = DamagersTeams.Player;
        damageDetector.GetComponent<Collider2D>().enabled = false;
    }
    IEnumerator BounceAwayCoroutine(Vector2 directionAway)
    {
        float timer = 0;
        ProjectileTf.up = directionAway;
        while(timer < bounceDuration)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / bounceDuration;
            float thisVelocity = Mathf.Lerp(bounceInitialSpeed, 0, normalizedTime);

            ProjectileTf.position += ProjectileTf.up * thisVelocity * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
    IEnumerator selfDestroyInSeconds(float secondsToDie)
    {
        yield return new WaitForSeconds(secondsToDie);
        destroyItself();
    }
    void destroyItself()
    {
        damageDealer.GetComponent<Collider2D>().enabled = false;
        damageDetector.GetComponent<Collider2D>().enabled = false;
        ProjectileTf.GetComponentInChildren<TrailRenderer>().emitting = false;
        ProjectileTf.GetComponent<Animator>().SetTrigger("Impact");
        Destroy(gameObject,2);
    }


    Coroutine currentMovement;
    IEnumerator followRotationCoroutine()
    {
        float timer = 0;
        while (timer < timeToReachFinalRadius)
        {
            timer += Time.deltaTime;
            float thisDistance = Mathf.Lerp(0, rotationRadius, timer / timeToReachFinalRadius);
            SetRotationRadiusDistance(thisDistance);

            GoRound();
            yield return null;
        }
        while (true)
        {
            GoRound();
            yield return null;
        }

        //
        void SetRotationRadiusDistance(float radius)
        {
            RotatingPos.localPosition = new Vector3(radius, 0, 0);
        }
        void GoRound()
        {
            RotationRoot.eulerAngles += new Vector3(0, 0, rotationSpeed_DegPerSec * Time.deltaTime);
            ProjectileTf.position = RotatingPos.position;
        }
    }
    
   
    
}
