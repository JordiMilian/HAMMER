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
    void onSomethingEntered(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(TagsCollection.Player_SinglePointCollider))
        {
            PlayersStepper.isStepping = true;
            PlayersStepper.followingEntityTf = collision.transform;
        }
        else if (collision.gameObject.CompareTag(TagsCollection.Enemy))
        {
            foreach (PuddleStepsPlayer pudler in SteppersGOList)
            {
                if (pudler.isStepping) { continue; }
                pudler.followingEntityTf = collision.transform;
                pudler.isStepping = true;
                break;
            }
        }
    }
    void onSomethingExited(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TagsCollection.Player_SinglePointCollider))
        {
            PlayersStepper.isStepping = false;
            PlayersStepper.followingEntityTf = null;
        }
        else if (collision.gameObject.CompareTag(TagsCollection.Enemy))
        {
            foreach (PuddleStepsPlayer pudler in SteppersGOList)
            {
                if(pudler.followingEntityTf == collision.transform)
                {
                    pudler.isStepping = false;
                    pudler.followingEntityTf = null;
                    break;
                }
            }
        }
    }



}
