using Pathfinding.Ionic.Zlib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddleSteps_Manager : MonoBehaviour
{
    [SerializeField] List<PuddleStepsPlayer> SteppersGOList = new List<PuddleStepsPlayer>();
    [SerializeField] PuddleStepsPlayer PlayersStepper;
    [SerializeField] List<Generic_OnTriggerEnterEvents> puddleTriggers = new List<Generic_OnTriggerEnterEvents>();

    private void OnEnable()
    {
        foreach (Generic_OnTriggerEnterEvents ontrigger in puddleTriggers)
        {
            ontrigger.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
            ontrigger.AddActivatorTag(TagsCollection.Enemy);
            ontrigger.OnTriggerEntered += onSomethingEntered;
            ontrigger.OnTriggerExited += onSomethingExited;
        }
    }
    void onSomethingEntered(object sender, Generic_OnTriggerEnterEvents.EventArgsCollisionInfo info)
    {
        Debug.Log("Some entered");
        if(info.Collision.gameObject.CompareTag(TagsCollection.Player_SinglePointCollider))
        {
            PlayersStepper.isStepping = true;
            PlayersStepper.followingEntityTf = info.Collision.transform;
        }
        else if (info.Collision.gameObject.CompareTag(TagsCollection.Enemy))
        {
            foreach (PuddleStepsPlayer pudler in SteppersGOList)
            {
                if (pudler.isStepping) { continue; }
                pudler.followingEntityTf = info.Collision.transform;
                pudler.isStepping = true;
                break;
            }
        }
    }
    void onSomethingExited(object sender, Generic_OnTriggerEnterEvents.EventArgsCollisionInfo info)
    {
        if (info.Collision.gameObject.CompareTag(TagsCollection.Player_SinglePointCollider))
        {
            PlayersStepper.isStepping = false;
            PlayersStepper.followingEntityTf = null;
        }
        else if (info.Collision.gameObject.CompareTag(TagsCollection.Enemy))
        {
            foreach (PuddleStepsPlayer pudler in SteppersGOList)
            {
                if(pudler.followingEntityTf == info.Collision.transform)
                {
                    pudler.isStepping = false;
                    pudler.followingEntityTf = null;
                    break;
                }
            }
        }
    }



}
