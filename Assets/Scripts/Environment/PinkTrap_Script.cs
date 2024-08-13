using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkTrap_Script : MonoBehaviour
{
    [SerializeField] Generic_AreaTriggerEvents areaTrigger;
    [SerializeField] Animator spikesAnimator;
    public bool areSpikesDeactivated;

    private void OnEnable()
    {
        //areaTrigger.AddActivatorTag(TagsCollection.Enemy_SinglePointCollider);
        areaTrigger.AddActivatorTag(TagsCollection.Player_SinglePointCollider);

        areaTrigger.onAreaActive += setSpikesAnimator;
        areaTrigger.onAreaUnactive += setSpikesAnimator;
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


}
