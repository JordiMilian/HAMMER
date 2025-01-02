using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnterTriggerCutscene : MonoBehaviour
{
    [SerializeField] Generic_OnTriggerEnterEvents enterRoomTrigger;
    [SerializeField] BaseCutsceneLogic thisCutsceneLogic;
    public bool hasCutscenePlayed;

    private void OnEnable()
    {
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
        CutscenesManager.Instance.AddCutscene(thisCutsceneLogic);
        hasCutscenePlayed = true;
    }
}
