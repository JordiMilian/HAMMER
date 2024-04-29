using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato_NewController : MonoBehaviour
{
    [SerializeField] AnimationClip ParaboleAnimation;
    [SerializeField] AudioClip TomatoImpactSFX;
     public void ThrowItself(GameObject DestinationGO, Vector3 initialPosition, Vector3 destinationPosition)
    {
        float ParaboleTime = ParaboleAnimation.length;
        StartCoroutine(MoveProjectileThroghTime(DestinationGO, initialPosition, destinationPosition, ParaboleTime));
    }
    IEnumerator MoveProjectileThroghTime(GameObject DestinationGO, Vector3 initialPosition, Vector3 destinationPosition, float timeToDestination)
    {
        //Get the vector between the origin and destination
        Vector3 projectileDirection = destinationPosition - initialPosition;
        float timer = 0;
        float adder = 0;
        //move according to the time to destination
        while (timer < timeToDestination)
        {
            timer += Time.deltaTime;

            adder = projectileDirection.magnitude * (timer / timeToDestination);
            Vector3 CurrentProjectilePosition = initialPosition + (projectileDirection.normalized * adder);

            transform.position = CurrentProjectilePosition;
            yield return null;
        }
        //when time is over, it landed
        OnLanded(DestinationGO);

    }
    void OnLanded(GameObject DestinationGO)
    {
        Animator DestinatioAnimator = DestinationGO.GetComponent<Animator>();
        DestinatioAnimator.SetTrigger("Landed");
        Game_AudioPlayerSingleton.Instance.playSFXclip(TomatoImpactSFX);
        Destroy(gameObject);
    }
}
