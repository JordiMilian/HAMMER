using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnterTriggerCutscene : MonoBehaviour
{
    [SerializeField] Generic_OnTriggerEnterEvents enterRoomTrigger;
    [SerializeField] GameObject CutsceneableHolder;
     ICutsceneable thisCutsceneable;
    public bool hasCutscenePlayed;

    private void OnValidate()
    {
        UsefullMethods.CheckIfGameobjectImplementsInterface<ICutsceneable>(ref CutsceneableHolder, ref thisCutsceneable);
    }
    private void OnEnable()
    {
        OnValidate();
        enterRoomTrigger.AddActivatorTag(Tags.Player_SinglePointCollider);
        enterRoomTrigger.OnTriggerEntered += callEntered;
    }
    private void OnDisable()
    {
        enterRoomTrigger.OnTriggerEntered -= callEntered;
    }
    void callEntered(Collider2D collision)
    {
        if (hasCutscenePlayed) { return; }
        CutscenesManager.Instance.AddCutsceneable(thisCutsceneable);
        hasCutscenePlayed = true;
    }
}
