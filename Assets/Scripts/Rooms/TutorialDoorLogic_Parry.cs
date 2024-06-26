using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoorLogic_Parry : BaseRoomWithDoorLogic
{
    Player_EventSystem playerEvents;
    int parriesDone;
    [SerializeField] int parriesToOpen;
    [SerializeField] Generic_DamageDealer MannequinDamageDealer;

    public override void OnEnable()
    {
        base.OnEnable();

        playerEvents = GameObject.Find(TagsCollection.MainCharacter).GetComponent<Player_EventSystem>(); //GUARRROO

        playerEvents.OnSuccessfulParry += Count1Parry;
    }
    void Count1Parry(object sender, Generic_EventSystem.SuccesfulParryInfo info)
    {
        if(info.ParriedDamageDealer == MannequinDamageDealer)
        {
            parriesDone++;
            if (parriesDone == parriesToOpen) 
            {
                RoomCompleted(true, true);
                playerEvents.OnSuccessfulParry -= Count1Parry;
            }
        }
    }
}
