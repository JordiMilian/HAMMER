using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Automation_ReplaceAnimationEventsTest : MonoBehaviour
{
    [Serialize]
    public List<AnimationClip> AnimationsToCheck;
    [SerializeField] string OriginalName;
    [SerializeField] string NewName;

    private void Start()
    {
        ReplaceEventName(OriginalName, NewName);
    }
    void ReplaceEventName(string originalName, string newName)
    {
        int replacementsCount = 0;

        foreach (AnimationClip clip in AnimationsToCheck)
        {
            List<AnimationEvent> newEventsList = new List<AnimationEvent>();

            foreach (AnimationEvent currentEvent in clip.events)
            {
                if (currentEvent.functionName == originalName)
                {
                    AnimationEvent replacerEvent = new AnimationEvent();
                    replacerEvent.functionName = newName;
                    replacerEvent.time = currentEvent.time;

                    newEventsList.Add(replacerEvent);                 

                    replacementsCount++;
                }
                else
                {
                    newEventsList.Add(currentEvent);
                }
            }
            clip.events = newEventsList.ToArray();
        }
        Debug.Log("Replacements done: " + replacementsCount);
    }
}
