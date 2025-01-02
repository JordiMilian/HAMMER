using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkTrap_Script : MonoBehaviour
{
    [SerializeField] Generic_AreaTriggerEvents areaTrigger;
    [SerializeField] Animator spikesAnimator;
    [SerializeField] RoomWithEnemiesLogic roomWithEnemiesLogic;
    [SerializeField] AudioClip ActivationSFX;
    [SerializeField] float delayBeforeSFX;
    [SerializeField] float delayBeforeShake;
    public bool areSpikesDeactivated;

    private void OnEnable()
    {
        //areaTrigger.AddActivatorTag(TagsCollection.Enemy_SinglePointCollider);
        areaTrigger.AddActivatorTag(Tags.Player_SinglePointCollider);

        areaTrigger.onAreaActive += setSpikesAnimator;
        areaTrigger.onAreaUnactive += setSpikesAnimator;
        if (roomWithEnemiesLogic != null) { roomWithEnemiesLogic.onRoomCompleted += unsubscribeFromEverything; }
    }
    private void OnDisable()
    {
        areaTrigger.onAreaActive -= setSpikesAnimator;
        areaTrigger.onAreaUnactive -= setSpikesAnimator;
    }
    void setSpikesAnimator()
    {
        if (areSpikesDeactivated) { return; }

        spikesAnimator.SetBool("SpikesOn", areaTrigger.isAreaActive);
        
    }
    void unsubscribeFromEverything(BaseRoomWithDoorLogic logic)
    {
        areSpikesDeactivated = true;
        spikesAnimator.SetBool("SpikesOn", false);
        spikesAnimator.SetTrigger("Deactivated");
        areaTrigger.onAreaActive -= setSpikesAnimator;
        areaTrigger.onAreaUnactive -= setSpikesAnimator;
    }
    public void EV_startTrapFeedback()
    {
        StartCoroutine(TrapFeedbackCoroutine());
    }
     IEnumerator TrapFeedbackCoroutine()
    {
        SFX_PlayerSingleton.Instance.playSFX(ActivationSFX, 0.1f);

        yield return new WaitForSeconds(delayBeforeShake);
        CameraShake.Instance.ShakeCamera(.5f, 0.07f);
    }

}
