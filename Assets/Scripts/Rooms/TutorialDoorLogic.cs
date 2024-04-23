using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoorLogic : BaseRoomLogic
{
     Player_EventSystem playerEvents;
    public enum TypesOfRoom
    {
        ParryRoom,FocusRoom
    }
    public TypesOfRoom typeOfRoom;
    int parriesDone;
    [SerializeField] int parriesToOpen;
    bool succesfullyFocused;
    public override void OnEnable()
    {
        base.OnEnable();

        playerEvents = GameObject.Find(TagsCollection.MainCharacter).GetComponent<Player_EventSystem>();

        switch (typeOfRoom)
        {
            case TypesOfRoom.ParryRoom:
                playerEvents.OnSuccessfulParry += Count1Parry;
                break;
            case TypesOfRoom.FocusRoom:
                playerEvents.OnFocusEnemy += FocusedEnemy;
                playerEvents.OnUnfocusEnemy += UnfocusedEnemy;
                break;
        }
    }
    void FocusedEnemy()
    {
        RoomCompleted(true, true);
        succesfullyFocused = true;
    }
    void UnfocusedEnemy()
    {
        //if (succesfullyFocused) { RoomCompleted(true, true); }
    }
    void Count1Parry(object sender, Generic_EventSystem.SuccesfulParryInfo info)
    {
        parriesDone++;
        if(parriesDone == parriesToOpen ) { RoomCompleted(true,true); }
    }

}
