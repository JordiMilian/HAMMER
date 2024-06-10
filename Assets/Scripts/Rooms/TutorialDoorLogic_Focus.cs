using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoorLogic_Focus : BaseRoomWithDoorLogic
{
    [SerializeField] GameObject FocusMannequin;
     Player_EventSystem playerEvents;

    public override void OnEnable()
    {
        base.OnEnable();

        playerEvents = GameObject.Find(TagsCollection.MainCharacter).GetComponent<Player_EventSystem>();

        playerEvents.OnFocusEnemy += FocusedEnemy;
    }
    void FocusedEnemy(GameObject focusedGO)
    {
        if(focusedGO == FocusMannequin)
        {
            RoomCompleted(true, true);
            playerEvents.OnFocusEnemy -= FocusedEnemy;
        }
    }
    

}
