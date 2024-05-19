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
        enterRoomTrigger.AddActivatorTag(TagsCollection.Player_SinglePointCollider);
        enterRoomTrigger.OnTriggerEntered += callEntered;
    }
    private void OnDisable()
    {
        enterRoomTrigger.OnTriggerEntered -= callEntered;
    }
    void callEntered(object sender, Generic_OnTriggerEnterEvents.EventArgsCollisionInfo args)
    {
        if (hasCutscenePlayed) { return; }
        thisCutsceneLogic.playThisCutscene();
        hasCutscenePlayed = true;
    }
    public interface IEnterRoomCutseneable
    {
        void playCutscene();
    }
}
