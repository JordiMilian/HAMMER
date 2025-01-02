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
            ontrigger.AddActivatorTag(Tags.Player_SinglePointCollider);
            ontrigger.AddActivatorTag(Tags.Enemy_SinglePointCollider);
            ontrigger.OnTriggerEntered += onSomethingEntered;
            ontrigger.OnTriggerExited += onSomethingExited;
        }
    }
    void onSomethingEntered(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(Tags.Player_SinglePointCollider))
        {
            PlayersStepper.isStepping = true;
            PlayersStepper.followingEntityTf = collision.transform;
        }
        else if (collision.gameObject.CompareTag(Tags.Enemy_SinglePointCollider))
        {
            Debug.Log("Enemy entered puddle: "+ collision.gameObject.name);
            foreach (PuddleStepsPlayer pudler in SteppersGOList)
            {
                if (pudler.followingEntityTf == collision.transform) { return; }
                if (pudler.isStepping) { continue; }
               

                pudler.followingEntityTf = collision.transform;
                pudler.isStepping = true;
                break;
            }
        }
    }
    void onSomethingExited(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.Player_SinglePointCollider))
        {
            PlayersStepper.isStepping = false;
            PlayersStepper.followingEntityTf = null;
        }
        else if (collision.gameObject.CompareTag(Tags.Enemy))
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
